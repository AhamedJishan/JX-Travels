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

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithOne(ur => ur.User);
            modelBuilder.Entity<User>(u => u.Property(i => i.Id).UseIdentityColumn(101, 1));

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.ArrivalAirport)
                .WithMany(a => a.FlightsArriving)
                .HasForeignKey(f => f.ArrivalAirportCode)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Airport>()
                .HasMany(f => f.FlightsArriving)
                .WithOne(a => a.ArrivalAirport)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Flight>()
                .HasOne(f => f.DepartureAirport)
                .WithMany(a => a.FlightsDeparting)
                .HasForeignKey(f => f.DepartureAirportCode)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Airport>()
                .HasMany(f => f.FlightsDeparting)
                .WithOne(a => a.DepartureAirport)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SeatInventory>()
                .HasOne<Flight>(si => si.Flight)
                .WithMany(f => f.SeatInventories)
                .HasForeignKey(si => si.FlightId)
                .OnDelete(DeleteBehavior.ClientNoAction);
            modelBuilder.Entity<Flight>()
                .HasMany(si => si.SeatInventories)
                .WithOne(f => f.Flight)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SeatInventory>()
                .HasKey(nameof(SeatInventory.FlightId), nameof(SeatInventory.Class));

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Airport> Airports { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<SeatInventory> SeatInventories { get; set; }
    }
}
