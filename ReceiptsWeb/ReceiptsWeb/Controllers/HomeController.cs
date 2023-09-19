using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using ReceiptsWeb.Models;
using System.Diagnostics;

namespace ReceiptsWeb.Controllers
{
	public class HomeController : Controller
	{
		//private readonly ILogger<HomeController> _logger;

		public HomeController(/*ILogger<HomeController> logger*/)
		{
			//_logger = logger;
		}

		public IActionResult Index()
		{
			return RedirectToAction("GroupProducts", "Products");
		}

		public IActionResult Tests()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		/// <summary>
		/// Set language and return to original url
		/// </summary>
		/// <param name="culture">new culture</param>
		/// <param name="returnUrl">original url</param>
		/// <returns></returns>
		[HttpPost]
		public IActionResult SetLanguage(string culture, string returnUrl)
		{
			Response.Cookies.Append(
				CookieRequestCultureProvider.DefaultCookieName,
				CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
				new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
			);

			return LocalRedirect(returnUrl);
		}
	}
}