using AppDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelWeb.Models;

public class BookingsController : Controller
{
	private readonly TravelWebPageContext _context;
	public BookingsController(TravelWebPageContext context)
	{
		_context = context;
	}

	[HttpGet]
	public async Task<IActionResult> Create(int tourId, int datePriceId)
	{
		var datePrice = await _context.DatePrices
			.Include(d => d.Tour)
			.FirstOrDefaultAsync(d => d.Id == datePriceId);
		if (datePrice == null) return NotFound();

		ViewBag.StartDate = datePrice.StartDate;
		ViewBag.EndDate = datePrice.EndDate;
		ViewBag.Price = datePrice.Price;
		ViewBag.TourName = datePrice.Tour?.Title;

		var booking = new Booking { TourId = tourId, DatePriceId = datePriceId };
		return View(booking);
	}

	[HttpPost]
	public async Task<IActionResult> Create(Booking booking)
	{
		if (!ModelState.IsValid)
		{
			var datePrice = await _context.DatePrices
				.Include(d => d.Tour)
				.FirstOrDefaultAsync(d => d.Id == booking.DatePriceId);

			ViewBag.StartDate = datePrice?.StartDate;
			ViewBag.EndDate = datePrice?.EndDate;
			ViewBag.Price = datePrice?.Price;
			ViewBag.TourName = datePrice?.Tour?.Title;

			return View(booking);
		}

		booking.CreatedAt = DateTime.Now;
		_context.Bookings.Add(booking);
		await _context.SaveChangesAsync();
		return RedirectToAction("Confirmation", new { name = booking.FullName, tour = booking.TourId });
	}

	public async Task<IActionResult> Confirmation(string name, int tour)
	{
		var tourDetails = await _context.Tours.FindAsync(tour);
		ViewBag.Name = name;
		ViewBag.TourName = tourDetails?.Title;
		return View();
	}
}