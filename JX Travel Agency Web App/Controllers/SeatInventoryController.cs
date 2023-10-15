using JX_Travel_Agency_Web_App.Data;
using JX_Travel_Agency_Web_App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JX_Travel_Agency_Web_App.Controllers
{
	public class SeatInventoryController : Controller
	{
		private readonly AppDbContext _db;
        public SeatInventoryController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> SeatInventory()
		{
			List<SeatInventory> seats = await _db.SeatInventories.ToListAsync();
			return View(seats);
		}

		public IActionResult AddSeat()
		{
			ViewBag.FlightList = _db.Flights.ToList();
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> AddSeat(SeatInventory seat)
		{
			ViewBag.FlightList = _db.Flights.ToList();

			ModelState.Remove("Flight");
			if (ModelState.IsValid)
			{
				await _db.SeatInventories.AddAsync(seat);
				await _db.SaveChangesAsync();

				TempData["msg"] = seat.Class + " Seat Added Successfully!";
				return RedirectToAction(nameof(SeatInventory));
			}
			return View(seat);
		}
	}
}
