using AppDbContext;
using Microsoft.AspNetCore.Mvc;
using TravelWeb.Models;

public class ServicesController : Controller
{
	private readonly TravelWebPageContext _context;

	public ServicesController(TravelWebPageContext context)
	{
		_context = context;
	}

	[HttpGet]
	public IActionResult Create(int tourId)
	{
		var service = new Service { TourId = tourId };
		return View(service);
	}

	[HttpPost]
	public async Task<IActionResult> Create(Service service)
	{
		_context.Services.Add(service);
		await _context.SaveChangesAsync();
		return RedirectToAction("Details", "Tours", new { id = service.TourId });
	}
}