using JX_Travel_Agency_Web_App.Data;
using JX_Travel_Agency_Web_App.Models;
using JX_Travel_Agency_Web_App.Models.QueryModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
			List<Flight>? flights= await _db.Flights.ToListAsync();
			return View();
		}

        public IActionResult Airports()
        {
            return View();
        }

        public IActionResult SeatInventory()
        {
            return View();
        }
    }
}
