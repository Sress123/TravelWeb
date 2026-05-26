using AppDbContext;
using Microsoft.AspNetCore.Mvc;
using TravelWeb.Models;

public class AccommodationsController : Controller
{
	private readonly TravelWebPageContext _context;

	public AccommodationsController(TravelWebPageContext context)
	{
		_context = context;
	}

	[HttpGet]
	public IActionResult Create(int tourId)
	{
		var accommodation = new Accommodation { TourId = tourId };
		return View(accommodation);
	}

	[HttpPost]
	public async Task<IActionResult> Create(Accommodation accommodation)
	{
		if (!ModelState.IsValid)
			return View(accommodation);

		_context.Accommodations.Add(accommodation);
		await _context.SaveChangesAsync();
		return RedirectToAction("Details", "Tours", new { id = accommodation.TourId });
	}
}