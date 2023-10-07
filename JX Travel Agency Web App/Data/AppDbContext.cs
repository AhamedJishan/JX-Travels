using JX_Travel_Agency_Web_App.Models;
using Microsoft.EntityFrameworkCore;

namespace JX_Travel_Agency_Web_App.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions options):base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SeatInventory>()
                .HasOne<Flight>(si => si.Flight)
                .WithMany(f => f.SeatInventories)
                .HasForeignKey(si=>si.FlightId);

            modelBuilder.Entity<SeatInventory>()
                .HasKey(nameof(SeatInventory.FlightId), nameof(SeatInventory.Class));
        }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<SeatInventory> SeatInventories { get; set; }
    }
}
