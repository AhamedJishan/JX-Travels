using JX_Travel_Agency_Web_App.Data;
using JX_Travel_Agency_Web_App.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JX_Travel_Agency_Web_App.Controllers
{
    public class HomeController : Controller
    {
        public readonly AppDbContext _db;
        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var airportList = _db.Airports.ToList();
            return View(airportList);
        }

    }
}