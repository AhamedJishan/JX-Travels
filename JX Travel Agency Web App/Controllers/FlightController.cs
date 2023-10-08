using JX_Travel_Agency_Web_App.Models;
using Microsoft.AspNetCore.Mvc;

namespace JX_Travel_Agency_Web_App.Controllers
{
	public class FlightController : Controller
	{
		[HttpPost]
		public IActionResult SearchFlights(FlightQueryModel flightQueryModel)
		{
			return View(flightQueryModel);
		}
	}
}
