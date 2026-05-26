using AppDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelWeb.Models;

namespace TravelWeb.Controllers
{
	public class DestinationsController : Controller
	{
		private readonly TravelWebPageContext _context;

		public DestinationsController(TravelWebPageContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			var destinations = _context.Destinations.OrderBy(d => d.Country).ToList();
			return View(destinations);
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpGet]
		[ActionName("Create")]
		public IActionResult CreateGet(int? id)
		{
			if (id != null)
			{
				var destination = _context.Destinations.FirstOrDefault(d => d.Id == id);
				if (destination == null) return NotFound();
				return View("Create", destination);
			}
			return View("Create", new Destination());
		}

		// POST - Save form (for both create and edit)
		[HttpPost]
		[ActionName("Create")]
		public async Task<IActionResult> CreatePost(Destination destination)
		{
			if (destination.PhotoFile != null && destination.PhotoFile.Length > 0)
			{
				var fileName = Guid.NewGuid().ToString() + Path.GetExtension(destination.PhotoFile.FileName);
				var savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/destinations", fileName);

				Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

				using (var stream = new FileStream(savePath, FileMode.Create))
				{
					await destination.PhotoFile.CopyToAsync(stream);
				}

				destination.PhotoCategory = "/images/destinations/" + fileName;
			}

			if (destination.Id == 0)
			{
				_context.Destinations.Add(destination);
			}
			else
			{
				var existing = _context.Destinations.FirstOrDefault(d => d.Id == destination.Id);
				if (existing != null)
				{
					existing.Country = destination.Country;
					existing.Destinationcategory = destination.Destinationcategory;
					existing.Description = destination.Description;

					if (!string.IsNullOrEmpty(destination.PhotoCategory))
					{
						existing.PhotoCategory = destination.PhotoCategory;
					}
				}
			}

			await _context.SaveChangesAsync();
			return RedirectToAction("Index");
		}

		public IActionResult Details(int id)
		{
			var destination = _context.Destinations
				.Include(d => d.Tours)
				.FirstOrDefault(d => d.Id == id);
			if (destination == null) return NotFound();
			return View(destination);
		}

		public IActionResult Delete(int id)
		{
			var destination = _context.Destinations.FirstOrDefault(d => d.Id == id);
			if (destination == null) return NotFound();
			_context.Destinations.Remove(destination);
			_context.SaveChanges();
			return RedirectToAction("Index");
		}
	}
}