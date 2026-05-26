using AppDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelWeb.Models;

namespace TravelWeb.Controllers
{
	public class DepartureController : Controller
	{
		private readonly TravelWebPageContext _context;
		public DepartureController(TravelWebPageContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var tours = await _context.Tours
				.Include(t => t.DatePrices)
				.OrderBy(t => Guid.NewGuid())
				.Take(15)
				.ToListAsync();
			return View(tours);
		}
	}
}