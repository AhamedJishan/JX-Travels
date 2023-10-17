using JX_Travel_Agency_Web_App.Data;
using JX_Travel_Agency_Web_App.Data.Enums;
using JX_Travel_Agency_Web_App.Models;
using JX_Travel_Agency_Web_App.Models.QueryModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace JX_Travel_Agency_Web_App.Controllers
{
	public class BookingController : Controller
	{
		private readonly AppDbContext _db;
        public BookingController(AppDbContext db)
        {
            _db = db;
        }
        
        public IActionResult Booking(string id)
        {
            List<string> parameters = id.Split("|").ToList();

			// storing flight ids for tickets
			TempData["FlightIdList"]= parameters[0];
            TempData["Class"]= parameters[1];
            TempData["TotalPrice"] = parameters[2];
            TempData.Keep("FlightIdList");
            TempData.Keep("Class");
            TempData.Keep("TotalPrice");

            Booking booking = new Booking()
            {
                TotalPrice = int.Parse(parameters[2].ToString())
            };
            _db.Add(booking);
            _db.SaveChanges();

			// storing booking ids for tickets
			TempData["BookingId"]= booking.BookingId;
            TempData.Keep("BookingId");

            return RedirectToAction(nameof(AddPassengers));
        }

        public IActionResult AddPassengers()
        {
            return View();
        }
        [HttpPost]
		public IActionResult AddPassengers(IFormCollection form)
		{
            if (ModelState.IsValid)
            {
                string passengerIdString = "";
                for (int i = 0; i < int.Parse(TempData.Peek("Passengers").ToString()); i++)
                {
                    Passenger passenger = new Passenger()
                    {
                        Name = form["Name"][i].ToString(),
						Age = int.Parse(form["Age"][i].ToString()),
						Gender = form["Gender"][i].ToString(),
						PhoneNumber = int.Parse(form["PhoneNumber"][i].ToString())
					};
                    _db.Passengers.Add(passenger);
					_db.SaveChanges();

                    passengerIdString+=passenger.PassengerId.ToString() + "_";
				}
				// storing passenger ids for tickets
				passengerIdString=passengerIdString.Remove(passengerIdString.Length-1);
                TempData["PassengerIds"]= passengerIdString;
                TempData.Keep("PassengerIds");

                return RedirectToAction(nameof(TicketConfirmation));
            }
            return View(form);
		}

		public IActionResult TicketConfirmation()
		{
            string TicketIds ="";
            foreach (string flightId in TempData.Peek("FlightIdList").ToString().Split("_"))
            {
                foreach (string passengerId in TempData.Peek("PassengerIds").ToString().Split("_"))
                {
                    Ticket ticket = new Ticket();
                    ticket.BookingId = int.Parse(TempData.Peek("BookingId").ToString());
                    ticket.PassengerId = int.Parse(passengerId);
					ticket.FlightId = int.Parse(flightId);
					ticket.Class = TempData.Peek("Class").ToString();
                    ticket.Price = int.Parse(TempData.Peek("TotalPrice").ToString());


					_db.Tickets.Add(ticket);
                    _db.SaveChanges();
                    TicketIds += ticket.TicketId.ToString();
                }
            }

            TempData["TickeIds"] = TicketIds;
            TempData.Keep("TicketIds");

            Booking? bookingData = _db.Bookings
                .Include(t=>t.Tickets)
                .ThenInclude(p=>p.Passenger)
                .FirstOrDefault(b=>b.BookingId==int.Parse(TempData.Peek("BookingId").ToString()));

			return View(bookingData);
		}
	}
}
