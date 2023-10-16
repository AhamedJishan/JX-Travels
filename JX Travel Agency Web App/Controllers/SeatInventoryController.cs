using JX_Travel_Agency_Web_App.Data;
using JX_Travel_Agency_Web_App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JX_Travel_Agency_Web_App.Controllers
{
	[Authorize(Roles ="Admin")]
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

		public IActionResult EditSeat(string id )
		{
			var seatKey = id.Split(' ');
			ViewBag.FlightList = _db.Flights.ToList();
			var seat = _db.SeatInventories.Where(s => s.FlightId.ToString().Equals(seatKey[0]) && s.Class.Equals(seatKey[1]));
			return View(seat.FirstOrDefault());
		}
		[HttpPost]
		public async Task<IActionResult> EditSeat(SeatInventory seat)
		{
			ViewBag.FlightList = _db.Flights.ToList();

			ModelState.Remove("Flight");
			if (ModelState.IsValid)
			{
				_db.SeatInventories.Update(seat);
				await _db.SaveChangesAsync();

				TempData["msg"] = seat.Class + " Seat Edited Successfully!";
				return RedirectToAction(nameof(SeatInventory));
			}
			return View(seat);
		}

		public IActionResult DeleteSeat(string id)
		{
			var seatKey = id.Split(' ');
			ViewBag.FlightList = _db.Flights.ToList();
			var seat = _db.SeatInventories.Where(s => s.FlightId.ToString().Equals(seatKey[0]) && s.Class.Equals(seatKey[1]));
			return View(seat.FirstOrDefault());
		}
		[HttpPost]
		public async Task<IActionResult> DeleteSeat(SeatInventory seat)
		{
			ViewBag.FlightList = _db.Flights.ToList();

			ModelState.Remove("Flight");
			if (ModelState.IsValid)
			{
				SeatInventory seatToBeDeleted = _db.SeatInventories
					.FirstOrDefault(s=>s.FlightId==seat.FlightId && s.Class == seat.Class);
				_db.Remove(seatToBeDeleted);
				await _db.SaveChangesAsync();

				TempData["msg"] = seat.Class + " Seat Deleted Successfully!";
				return RedirectToAction(nameof(SeatInventory));
			}
			return View(seat);
		}
	}
}







