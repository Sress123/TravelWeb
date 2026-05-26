using AppDbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TravelWeb.Models;

namespace TravelWeb.Controllers
{
    public class HomeController : Controller
    {
		private readonly TravelWebPageContext _context;

		public HomeController(TravelWebPageContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var tours = await _context.Tours
				.OrderBy(t => Guid.NewGuid())
				.Take(8)
				.ToListAsync();
			ViewBag.Tours = tours;
			return View();
		}

		[HttpGet]
		public IActionResult Login()
        {
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> Login(string email, string password)
		{
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

			if(user == null)
			{
				ViewBag.Error = "Invalid email or password";
				return View();
			}

			HttpContext.Session.SetString("UserName", user.FullName);
			HttpContext.Session.SetString("UserRole", user.Role ?? "User");
			HttpContext.Session.SetInt32("UserId", user.Id);

			if (user.Role == "Admin")
				return RedirectToAction("Index", "Home");

			return RedirectToAction("Index", "Home");
		}

		public IActionResult Contact()
		{
			return View();
		}

		public IActionResult About()
		{
			return View();
		}

		public async Task<IActionResult> Reviews()
		{
			var reviews = await _context.Reviews
				.OrderByDescending(r => r.Createdat)
				.ToListAsync();
			return View(reviews);
		}

		[HttpPost]
		public async Task<IActionResult> SubmitReview(Review review)
		{
			review.Createdat = DateTime.Now;
			review.Isgenuine = Request.Form["isgenuine"] == "on";
			_context.Reviews.Add(review);
			await _context.SaveChangesAsync();
			return RedirectToAction("Reviews");
		}

		public IActionResult eBook()
		{
			return View();
		}

		public IActionResult Logout()
		{
			HttpContext.Session.Clear();
			return RedirectToAction("Login");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
