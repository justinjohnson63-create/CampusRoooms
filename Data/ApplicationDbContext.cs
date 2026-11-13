using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CampusRooms.Models;

namespace CampusRooms.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; } = null!;
        public DbSet<Building> Buildings { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Room>()
                .HasIndex(r => r.Code)
                .IsUnique();

            // Seed Buildings
            builder.Entity<Building>().HasData(
                new Building { Id = 1, Name = "Main Block" },
                new Building { Id = 2, Name = "Science Block" },
                new Building { Id = 3, Name = "Library" },
                new Building { Id = 4, Name = "Sports Complex" }
            );

            // Seed Rooms
            builder.Entity<Room>().HasData(
                new Room { Id = 1, Code = "MB-101", Name = "Lecture Theatre A", Seats = 120, HasProjector = true, BuildingId = 1 },
                new Room { Id = 2, Code = "MB-204", Name = "Seminar Room", Seats = 30, HasProjector = false, BuildingId = 1 },
                new Room { Id = 3, Code = "SB-110", Name = "Chemistry Lab", Seats = 24, HasProjector = true, BuildingId = 2 },
                new Room { Id = 4, Code = "SB-215", Name = "Computer Lab 2", Seats = 35, HasProjector = true, BuildingId = 2 },
                new Room { Id = 5, Code = "LIB-01", Name = "Study Room 1", Seats = 8, HasProjector = false, BuildingId = 3 }
            );
        }
    }
}
