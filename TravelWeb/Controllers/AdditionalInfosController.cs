using AppDbContext;
using Microsoft.AspNetCore.Mvc;
using TravelWeb.Models;

public class AdditionalInfosController : Controller
{
	private readonly TravelWebPageContext _context;

	public AdditionalInfosController(TravelWebPageContext context)
	{
		_context = context;
	}

	[HttpGet]
	public IActionResult Create(int tourId)
	{
		var info = new AdditionalInfo { TourId = tourId };
		return View(info);
	}

	[HttpPost]
	public async Task<IActionResult> Create(AdditionalInfo info)
	{
		if (!ModelState.IsValid)
			return View(info);

		_context.AdditionalInfos.Add(info);
		await _context.SaveChangesAsync();
		return RedirectToAction("Details", "Tours", new { id = info.TourId });
	}
}