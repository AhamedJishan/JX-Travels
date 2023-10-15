using JX_Travel_Agency_Web_App.Data;
using JX_Travel_Agency_Web_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JX_Travel_Agency_Web_App.Controllers
{
	public class AirportController : Controller
	{
		private readonly AppDbContext _db;
        public AirportController(AppDbContext db)
        {
            _db= db;
        }
        public async Task<IActionResult> Airports()
		{
			List<Airport> airports = await _db.Airports.ToListAsync();
			return View(airports);
		}


		public IActionResult AddAirport()
		{
			ViewBag.AirportList = _db.Airports.ToList();
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> AddAirport(Airport airport)
		{
			if (ModelState.IsValid)
			{
				Airport airportEntry = airport;
				
				await _db.Airports.AddAsync(airport);
				await _db.SaveChangesAsync();

				TempData["msg"]="Airport -"+airportEntry.Code+ " Added Successfully!";
				return RedirectToAction(nameof(Airports));
			}
			return View(airport);
		}

		public IActionResult EditAirport(string id)
		{
			ViewBag.AirportList = _db.Airports.ToList();
			Airport airport = _db.Airports.FirstOrDefault(a => a.Code == id);
			return View(airport);
		}
		[HttpPost]
		public async Task<IActionResult> EditAirport(Airport airport)
		{
			if (ModelState.IsValid)
			{	
				_db.Airports.Update(airport);
				await _db.SaveChangesAsync();

				TempData["msg"] = "Airport -" + airport.Code + " Edited successfully!";

				return RedirectToAction(nameof(Airports));
			}
			return View(airport);
		}

		public IActionResult DeleteAirport(string id)
		{
			ViewBag.AirportList = _db.Airports.ToList();
			Airport airport = _db.Airports.FirstOrDefault(a => a.Code == id);
			return View(airport);
		}
		[HttpPost]
		public async Task<IActionResult> DeleteAirport(Airport airport)
		{
			if (ModelState.IsValid)
			{
				Airport airportToBeDeleted = _db.Airports.FirstOrDefault(a => a.Code == airport.Code);
				if (true)
				{
					// Cascading problem fix
					var flights = _db.Flights.Where(f=>f.ArrivalAirportCode==airportToBeDeleted.Code);
					_db.Flights.RemoveRange(flights);

					_db.Airports.Remove(airportToBeDeleted);
					await _db.SaveChangesAsync();

					TempData["msg"] = "Airport -" + airportToBeDeleted.Code + " Deleted successfully!";

					return RedirectToAction(nameof(Airports));
				}
			}
			return View(airport);
		}


		//public async Task<IActionResult> SeatInventory()
		//{
		//	return View();
		//}
	}
}
