using JX_Travel_Agency_Web_App.Models;
using Microsoft.EntityFrameworkCore;

namespace JX_Travel_Agency_Web_App.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options):base(options)
        {
        }

        public DbSet<Airport> Airports { get; set; }
    }
}
