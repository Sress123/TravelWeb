using AppDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelWeb.Models;

namespace TravelWeb.Controllers
{
	public class TopToursController : Controller
	{
		private readonly TravelWebPageContext _context;
		public TopToursController(TravelWebPageContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index(string? destination, string? region, string? grade, string? duration)
		{
			var query = _context.Tours
				.Include(t => t.Destination)
				.AsQueryable();

			if (!string.IsNullOrEmpty(destination))
				query = query.Where(t => t.Destination != null && t.Destination.Country == destination);

			if (!string.IsNullOrEmpty(region))
				query = query.Where(t => t.Region == region);

			if (!string.IsNullOrEmpty(grade))
				query = query.Where(t => t.Grade == grade);

			if (!string.IsNullOrEmpty(duration))
				query = query.Where(t => t.Duration != null && t.Duration.Contains(duration));

			var tours = await query.ToListAsync();

			// Populate filter dropdowns
			ViewBag.Destinations = await _context.Destinations
				.Select(d => d.Country).Distinct().ToListAsync();
			ViewBag.Regions = await _context.Tours
				.Where(t => t.Region != null)
				.Select(t => t.Region).Distinct().ToListAsync();
			ViewBag.Grades = await _context.Tours
				.Where(t => t.Grade != null)
				.Select(t => t.Grade).Distinct().ToListAsync();

			// Keep selected filters
			ViewBag.SelectedDestination = destination;
			ViewBag.SelectedRegion = region;
			ViewBag.SelectedGrade = grade;
			ViewBag.SelectedDuration = duration;

			return View(tours);
		}
	}
}