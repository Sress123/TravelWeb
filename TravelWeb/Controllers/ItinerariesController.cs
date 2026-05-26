using AppDbContext;
using Microsoft.AspNetCore.Mvc;
using TravelWeb.Models;

namespace TravelWeb.Controllers
{
	public class ItinerariesController : Controller
	{
		private readonly TravelWebPageContext _context;

		public ItinerariesController(TravelWebPageContext context)
		{
			_context = context;
		}

		// GET
		[HttpGet]
		[ActionName("Create")]
		public IActionResult CreateGet(int tourId, int? id)
		{
			if (id != null)
			{
				var existing = _context.Itineraries.FirstOrDefault(i => i.Id == id);
				if (existing == null) return NotFound();
				return View("Create", existing);
			}

			// Auto set next day number
			var nextDay = _context.Itineraries
				.Where(i => i.TourId == tourId)
				.Count() + 1;

			return View("Create", new Itinerary { TourId = tourId, DayNumber = nextDay });
		}

		// POST
		[HttpPost]
		[ActionName("Create")]
		public async Task<IActionResult> CreatePost(Itinerary itinerary)
		{
			if (itinerary.Id == 0)
			{
				_context.Itineraries.Add(itinerary);
			}
			else
			{
				var existing = _context.Itineraries.FirstOrDefault(i => i.Id == itinerary.Id);
				if (existing != null)
				{
					existing.DayNumber = itinerary.DayNumber;
					existing.Title = itinerary.Title;
					existing.Description = itinerary.Description;
				}
			}

			await _context.SaveChangesAsync();
			return RedirectToAction("Details", "Tours", new { id = itinerary.TourId });
		}

		// DELETE
		public IActionResult Delete(int id, int tourId)
		{
			var itinerary = _context.Itineraries.FirstOrDefault(i => i.Id == id);
			if (itinerary != null)
			{
				_context.Itineraries.Remove(itinerary);
				_context.SaveChanges();
			}
			return RedirectToAction("Details", "Tours", new { id = tourId });
		}
	}
}