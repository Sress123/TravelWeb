using AppDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using TravelWeb.Models;

[Route("Gallaries")]
public class GallaryController : Controller
{
	private readonly TravelWebPageContext _context;
	private readonly IWebHostEnvironment _env;

	public GallaryController(TravelWebPageContext context, IWebHostEnvironment env)
	{
		_context = context;
		_env = env;
	}

	[HttpGet("Create")]
	public IActionResult Create(int tourId)
	{
		var gallary = new Gallary { TourId = tourId };
		return View(gallary);
	}

	[HttpPost("Create")]
	public async Task<IActionResult> Create(Gallary gallary, IFormFile photoFile)
	{
		if (photoFile != null && photoFile.Length > 0)
		{
			var folder = Path.Combine(_env.WebRootPath, "images", "gallery");
			Directory.CreateDirectory(folder);

			var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photoFile.FileName);
			var filePath = Path.Combine(folder, fileName);

			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await photoFile.CopyToAsync(stream);
			}

			gallary.PhotoPath = "/images/gallery/" + fileName;
		}

		_context.Gallaries.Add(gallary);
		await _context.SaveChangesAsync();
		return RedirectToAction("Details", "Tours", new { id = gallary.TourId });
	}
}