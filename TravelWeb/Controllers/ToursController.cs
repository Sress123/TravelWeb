using AppDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using TravelWeb.Models;

namespace TravelWeb.Controllers
{
	public class ToursController : Controller
	{
		private readonly TravelWebPageContext _context;
		private readonly IWebHostEnvironment _env;
		private readonly ICompositeViewEngine _viewEngine;

		public ToursController(TravelWebPageContext context, IWebHostEnvironment env, ICompositeViewEngine viewEngine)
		{
			_context = context;
			_env = env;
			_viewEngine = viewEngine;
		}

		[HttpGet]
		[ActionName("Create")]
		public IActionResult CreateGet(int? destinationId, int? id)
		{
			if (id != null)
			{
				var existing = _context.Tours.FirstOrDefault(t => t.Id == id);
				if (existing == null) return NotFound();
				return View("Create", existing);
			}
			return View("Create", new Tour { DestinationId = destinationId });
		}

		// POST - Save (Create or Edit)
		[HttpPost]
		[ActionName("Create")]
		public async Task<IActionResult> CreatePost(Tour tour)
		{
			if (tour.PhotoFile != null && tour.PhotoFile.Length > 0)
			{
				var fileName = Guid.NewGuid() + Path.GetExtension(tour.PhotoFile.FileName);
				var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/tours", fileName);
				Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);
				using var stream = new FileStream(savePath, FileMode.Create);
				await tour.PhotoFile.CopyToAsync(stream);
				tour.PhotoPath = "/images/tours/" + fileName;
			}

			if (tour.RouteMapPhotoFile != null && tour.RouteMapPhotoFile.Length > 0)
			{
				var fileName = Guid.NewGuid() + Path.GetExtension(tour.RouteMapPhotoFile.FileName);
				var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/tours", fileName);
				Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);
				using var stream = new FileStream(savePath, FileMode.Create);
				await tour.RouteMapPhotoFile.CopyToAsync(stream);
				tour.RouteMapPhotoPath = "/images/tours/" + fileName;
			}

			if (tour.Id == 0)
			{
				// CREATE
				_context.Tours.Add(tour);
			}
			else
			{
				// EDIT
				var existing = _context.Tours.FirstOrDefault(t => t.Id == tour.Id);
				if (existing != null)
				{
					existing.Title = tour.Title;
					existing.Description = tour.Description;
					existing.Duration = tour.Duration;
					existing.Grade = tour.Grade;
					existing.BestSeasons = tour.BestSeasons;
					existing.Altitude = tour.Altitude;
					existing.Region = tour.Region;
					existing.Accommodation = tour.Accommodation;

					if (!string.IsNullOrEmpty(tour.PhotoPath))
						existing.PhotoPath = tour.PhotoPath;

					if (!string.IsNullOrEmpty(tour.RouteMapPhotoPath))
						existing.RouteMapPhotoPath = tour.RouteMapPhotoPath;
				}
			}

			await _context.SaveChangesAsync();
			return RedirectToAction("Details", "Destinations", new { id = tour.DestinationId });
		}

		// DELETE
		public IActionResult Delete(int id, int destinationId)
		{
			var tour = _context.Tours.FirstOrDefault(t => t.Id == id);
			if (tour != null)
			{
				_context.Tours.Remove(tour);
				_context.SaveChanges();
			}
			return RedirectToAction("Details", "Destinations", new { id = destinationId });
		}

		// Tour Details
		public IActionResult Details(int id)
		{
			var tour = _context.Tours
				.Include(t => t.Destination)
				.Include(t => t.Itineraries.OrderBy(i => i.DayNumber))
				.Include(t => t.DatePrices)
				.Include(t => t.Services)
				.Include(t => t.Accommodations)
				.Include(t => t.AdditionalInfos)
				.Include(t => t.Gallaries)
				.FirstOrDefault(t => t.Id == id);

			if (tour == null) return NotFound();
			return View(tour);
		}

		public async Task<IActionResult> DownloadPdf(int id)
		{
			var tour = await _context.Tours
				.Include(t => t.Destination)
				.Include(t => t.Itineraries)
				.Include(t => t.Services)
				.Include(t => t.DatePrices)
				.Include(t => t.Accommodations)
				.FirstOrDefaultAsync(t => t.Id == id);

			if (tour == null) return NotFound();

			// Load images as bytes
			byte[]? tourPhoto = null;
			byte[]? routeMapPhoto = null;
			byte[]? logoPhoto = null;

			if (!string.IsNullOrEmpty(tour.PhotoPath))
			{
				var path = Path.Combine(_env.WebRootPath, tour.PhotoPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
				if (System.IO.File.Exists(path)) tourPhoto = await System.IO.File.ReadAllBytesAsync(path);
			}

			if (!string.IsNullOrEmpty(tour.RouteMapPhotoPath))
			{
				var path = Path.Combine(_env.WebRootPath, tour.RouteMapPhotoPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
				if (System.IO.File.Exists(path)) routeMapPhoto = await System.IO.File.ReadAllBytesAsync(path);
			}

			var logoPath = Path.Combine(_env.WebRootPath, "images", "logo.png");
			if (System.IO.File.Exists(logoPath)) logoPhoto = await System.IO.File.ReadAllBytesAsync(logoPath);

			var pdf = Document.Create(container =>
			{
				container.Page(page =>
				{
					page.Size(PageSizes.A4);
					page.Margin(40);
					page.DefaultTextStyle(x => x.FontSize(11));

					// HEADER
					page.Header().Column(col =>
					{
						col.Item().AlignCenter().Text(tour.Title).FontSize(22).Bold();
						col.Item().AlignCenter().Text($"Duration: {tour.Duration} | Region: {tour.Region}")
							.FontSize(11).FontColor("#666666");
						col.Item().PaddingTop(5).LineHorizontal(2).LineColor("#c0392b");
					});

					page.Content().PaddingTop(20).Column(col =>
					{
						// TOUR PHOTO
						if (tourPhoto != null)
						{
							col.Item().PaddingBottom(15).Image(tourPhoto).FitWidth();
						}

						// TRIP FACTS
						col.Item().Text("Trip Facts").FontSize(14).Bold().FontColor("#c0392b");
						col.Item().PaddingBottom(5).LineHorizontal(1).LineColor("#c0392b");
						col.Item().PaddingBottom(10).Table(table =>
						{
							table.ColumnsDefinition(c =>
							{
								c.RelativeColumn();
								c.RelativeColumn();
								c.RelativeColumn();
							});

							void AddFact(string label, string? value)
							{
								table.Cell().Padding(5).Column(c =>
								{
									c.Item().Text(label).Bold().FontColor("#c0392b").FontSize(10);
									c.Item().Text(value ?? "-").FontSize(11);
								});
							}

							AddFact("Activities", tour.Destination?.Destinationcategory);
							AddFact("Region", tour.Region);
							AddFact("Duration", tour.Duration);
							AddFact("Grade", tour.Grade);
							AddFact("Best Seasons", tour.BestSeasons);
							AddFact("Highest Elevation", tour.Altitude);
							AddFact("Accommodation", tour.Accommodation);
						});

						// OVERVIEW
						col.Item().PaddingTop(10).Text("Trip Overview").FontSize(14).Bold().FontColor("#c0392b");
						col.Item().PaddingBottom(5).LineHorizontal(1).LineColor("#c0392b");
						col.Item().PaddingBottom(10).Text(tour.Description ?? "").FontSize(11);

						// ITINERARY
						if (tour.Itineraries.Any())
						{
							col.Item().PaddingTop(10).Text("Itinerary In Detail").FontSize(14).Bold().FontColor("#c0392b");
							col.Item().PaddingBottom(5).LineHorizontal(1).LineColor("#c0392b");
							foreach (var day in tour.Itineraries.OrderBy(i => i.DayNumber))
							{
								col.Item().PaddingTop(8).Text($"Day {day.DayNumber}: {day.Title}").Bold().FontSize(12);
								col.Item().Background("#f8f8f8").Padding(8).Text(day.Description ?? "").FontSize(11);
							}
						}

						// SERVICES INCLUDED
						if (tour.Services.Any())
						{
							var service = tour.Services.First();

							col.Item().PaddingTop(10).Text("Services Included").FontSize(14).Bold().FontColor("#c0392b");
							col.Item().PaddingBottom(5).LineHorizontal(1).LineColor("#c0392b");
							foreach (var item in service.IncludeServices?.Split('\n') ?? Array.Empty<string>())
							{
								if (!string.IsNullOrWhiteSpace(item))
									col.Item().Text($"✓  {item.Trim()}").FontColor("#27ae60").FontSize(11);
							}

							col.Item().PaddingTop(10).Text("Services Not Included").FontSize(14).Bold().FontColor("#c0392b");
							col.Item().PaddingBottom(5).LineHorizontal(1).LineColor("#c0392b");
							foreach (var item in service.NotIncludeServices?.Split('\n') ?? Array.Empty<string>())
							{
								if (!string.IsNullOrWhiteSpace(item))
									col.Item().Text($"✗  {item.Trim()}").FontColor("#c0392b").FontSize(11);
							}
						}

						// ROUTE MAP
						if (routeMapPhoto != null)
						{
							col.Item().PaddingTop(20).Text("Route Map").FontSize(14).Bold().FontColor("#c0392b");
							col.Item().PaddingBottom(5).LineHorizontal(1).LineColor("#c0392b");
							col.Item().PaddingTop(10).Image(routeMapPhoto).FitWidth();
						}

						// COMPANY CARD
						col.Item().PaddingTop(30).Border(1).BorderColor("#e0e0e0").Padding(20).Column(card =>
						{
							// Logo
							if (logoPhoto != null)
							{
								card.Item().AlignCenter().Width(80).Image(logoPhoto);
							}

							card.Item().PaddingTop(10).AlignCenter()
								.Text("Any Question?").Bold().FontSize(13);

							card.Item().AlignCenter()
								.Text("Feel free to call our travel experts.")
								.FontColor("#666666").FontSize(11);

							card.Item().PaddingTop(10).AlignCenter()
								.Text("📞  +977 9843056391").FontSize(11);

							card.Item().AlignCenter()
								.Text("✉  info@prabeenGole.com").FontSize(11);
						});

					});

					// FOOTER
					page.Footer().PaddingTop(10).LineHorizontal(1).LineColor("#c0392b");
					//page.Footer().AlignCenter().Text(text =>
					//{
					//	text.Span("Kathmandu-16, Bohoratar, Nepal  |  Tel: +977 9801089018  |  info@asiaticroads.com")
					//		.FontSize(9).FontColor("#666666");
					//});
				});
			}).GeneratePdf();

			var fileName = $"{tour.Title.Replace(" ", "_")}.pdf";
			return File(pdf, "application/pdf", fileName);
		}
	}
}
