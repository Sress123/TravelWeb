using AppDbContext;
using Microsoft.AspNetCore.Mvc;
using TravelWeb.Models;

public class DatePricesController : Controller
{
	private readonly TravelWebPageContext _context;

	public DatePricesController(TravelWebPageContext context)
	{
		_context = context;
	}

	[HttpGet]
	public IActionResult Create(int tourId)
	{
		var datePrice = new DatePrice { TourId = tourId };
		return View(datePrice);
	}

	[HttpPost]
	public async Task<IActionResult> Create(DatePrice datePrice)
	{
		if (!ModelState.IsValid)
			return View(datePrice);

		_context.DatePrices.Add(datePrice);
		await _context.SaveChangesAsync();
		return RedirectToAction("Details", "Tours", new { id = datePrice.TourId });
	}
}