using JX_Travel_Agency_Web_App.Data;
using JX_Travel_Agency_Web_App.Models;
using JX_Travel_Agency_Web_App.Models.QueryModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace JX_Travel_Agency_Web_App.Controllers
{
    public class FlightController : Controller
	{
		private readonly AppDbContext _db;
        public FlightController(AppDbContext db)
        {
            _db = db;
        }

        [HttpPost, Authorize]
		public IActionResult SearchFlights(FlightQueryModel flightQuery)
		{
			var flightsList = _db.Flights
				.Include(f => f.DepartureAirport)
				.Include(f => f.ArrivalAirport)
				.Include(f => f.SeatInventories.Where(x => x.Class == flightQuery.Class.ToString()))
				.ToList();

			List<List<Flight>> filteredListOfFLights = new List<List<Flight>>();

			List<Flight> DepartureDateFilteredFlights = new List<Flight>();
			List<Flight> ClassFilteredFlightList= new List<Flight>();

			List<Flight> froms = new List<Flight>();
			List<Flight> tos = new List<Flight>();
			List<Flight> tempFLights = new List<Flight>();

			// Class Filter
			foreach (var flight in flightsList)
			{
				foreach (var seat in flight.SeatInventories)
				{
					if (seat.Class==flightQuery.Class.ToString())
					{
						ClassFilteredFlightList.Add(flight);
						break;
					}
				}
			}

			// Departure filter
			foreach (var flight in ClassFilteredFlightList)
			{
				if (flight.DepartureTime.ToString("dd-MM-yyyy")
					.Equals(flightQuery.DepartureDate.ToString("dd-MM-yyyy")))
				{
					DepartureDateFilteredFlights.Add(flight);
					continue;
				}
			}

			// No Stop flights
			foreach (var flight in DepartureDateFilteredFlights)
			{
				if (flight.DepartureAirportCode.Equals(flightQuery.From.Substring(0,3)) &&
					flight.ArrivalAirportCode.Equals(flightQuery.To.Substring(0, 3)))
				{
					List<Flight> noStopFLight = new List<Flight>();
					noStopFLight.Add(flight);
					filteredListOfFLights.Add(noStopFLight);
					continue;
				}
				else if (flight.DepartureAirportCode.Equals(flightQuery.From.Substring(0, 3)))
                {
                    froms.Add(flight);
					continue;
                }
				else if (flight.ArrivalAirportCode.Equals(flightQuery.To.Substring(0, 3)))
				{
					tos.Add(flight);
				} else
				{
					tempFLights.Add(flight);
				}
			}

			List<Flight> fromFlightTrash= new List<Flight>();
			List<Flight> toFlightTrash = new List<Flight>();

			// One StopFlights
			foreach (var fromFlight in froms)
			{
				List<Flight> oneStopFlight= new List<Flight>();

				bool oneStopFlightFound = false;

				foreach (var toFlight in tos)
                {
					
					if (fromFlight.ArrivalAirportCode.Equals(toFlight.DepartureAirportCode))
					{
						oneStopFlight.Add(fromFlight);
						oneStopFlight.Add(toFlight);

						oneStopFlightFound = true;

						fromFlightTrash.Add(fromFlight);
						toFlightTrash.Add(toFlight);
						break;
					}
                }
				if (oneStopFlightFound) filteredListOfFLights.Add(oneStopFlight);
            }

			foreach (var trashFlight in fromFlightTrash)
			{
				foreach (var flight in froms)
				{
					if (flight==trashFlight)
					{
						froms.Remove(flight);
						break;
					}
				}
			}
			foreach (var trashFlight in toFlightTrash)
			{
				foreach (var flight in tos)
				{
					if (flight == trashFlight)
					{
						tos.Remove(flight);
						break;
					}
				}
			}


			// Two Stop Flights
			if (froms.Count>=1 && tos.Count>=1)
			{
				List<Flight> twoStopFLights= new List<Flight>();
				foreach (var fromFlight in froms)
				{
					foreach(var toFlight in tos)
					{
						foreach (var midFlight in tempFLights)
						{
							bool Break = false;
							if (fromFlight.ArrivalAirportCode.Equals(midFlight.DepartureAirportCode) && 
								toFlight.DepartureAirportCode.Equals(midFlight.ArrivalAirportCode))
							{
								twoStopFLights.Add(fromFlight);
								twoStopFLights.Add(midFlight);
								twoStopFLights.Add(toFlight);
								Break= true;
								break;
							}
						}
					}
					filteredListOfFLights.Add(twoStopFLights);
				}
			}
			
			return View(filteredListOfFLights);
		}

		public async Task<IActionResult> Flights()
		{
			List<Flight>? flights= await _db.Flights
				.Include(f=>f.ArrivalAirport)
				.Include(f => f.DepartureAirport)
				.ToListAsync();
			return View(flights);
		}
		
		public IActionResult AddFlight()
		{
			ViewBag.AirportList=_db.Airports.ToList();
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> AddFlight(IFormCollection form)
		{
			if (ModelState.IsValid)
			{
				Flight flightEntry = new Flight();
				flightEntry.AirlineName = form["AirlineName"];
				flightEntry.FlightNumber = form["FlightNumber"];
				flightEntry.AircraftType = form["AircraftType"];
				flightEntry.DepartureAirportCode = form["DepartureAirport"].ToString().Substring(0,3);
				flightEntry.ArrivalAirportCode = form["ArrivalAirport"].ToString().Substring(0,3);

				flightEntry.DepartureTime = DateTime.Parse(form["DepartureDate"].ToString() +" "+ form["DepartureTime"].ToString());
				flightEntry.ArrivalTime = DateTime.Parse(form["ArrivalDate"].ToString() + " " + form["ArrivalTime"].ToString());

				await _db.Flights.AddAsync(flightEntry);
				await _db.SaveChangesAsync();

				return RedirectToAction(nameof(Flights));
			}
			return View(form);
		}

		public IActionResult EditFlight(int id)
		{
			TempData["FlightId"]=id;
			ViewBag.AirportList = _db.Airports.ToList();
			var flight=_db.Flights.FirstOrDefault(f=>f.FlightId == id);
			return View(flight);
		}
		[HttpPost]
		public async Task<IActionResult> EditFlight(IFormCollection form)
		{
			if (ModelState.IsValid)
			{
				int flightID = (int)TempData["FlightId"];
				Flight flightEntry = _db.Flights.FirstOrDefault(f=>f.FlightId==flightID);
				flightEntry.AirlineName = form["AirlineName"];
				flightEntry.FlightNumber = form["FlightNumber"];
				flightEntry.AircraftType = form["AircraftType"];
				flightEntry.DepartureAirportCode = form["DepartureAirport"].ToString().Substring(0, 3);
				flightEntry.ArrivalAirportCode = form["ArrivalAirport"].ToString().Substring(0, 3);

				flightEntry.DepartureTime = DateTime.Parse(form["DepartureDate"].ToString() + " " + form["DepartureTime"].ToString());
				flightEntry.ArrivalTime = DateTime.Parse(form["ArrivalDate"].ToString() + " " + form["ArrivalTime"].ToString());

				_db.Flights.Update(flightEntry);
				await _db.SaveChangesAsync();

				TempData["msg"] = "Flight -" + flightID.ToString() + " Edited successfully!";

				return RedirectToAction(nameof(Flights));
			}
			return View(form);
		}

		public IActionResult DeleteFlight(int id)
		{
			TempData["FlightId"] = id;
			ViewBag.AirportList = _db.Airports.ToList();
			var flight = _db.Flights.FirstOrDefault(f => f.FlightId == id);
			return View(flight);
		}
		[HttpPost]
		public async Task<IActionResult> DeleteFlight(IFormCollection form)
		{
			if (ModelState.IsValid)
			{
				int flightID = (int)TempData["FlightId"];
				Flight flightEntry = _db.Flights.FirstOrDefault(f => f.FlightId == flightID);
				if (flightEntry != null)
				{
					_db.Flights.Remove(flightEntry);
					await _db.SaveChangesAsync();

					TempData["msg"] = "Flight -" + flightID.ToString() + " Deleted successfully!";

					return RedirectToAction(nameof(Flights));
				}
			}
			return View(form);
		}
		
    }
}
