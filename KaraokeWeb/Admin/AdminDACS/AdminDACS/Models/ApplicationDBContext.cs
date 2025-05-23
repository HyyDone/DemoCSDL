using AdminDACS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdminDACS.Models;

namespace AdminDACS.Models
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomsTypes { get; set; }
        public DbSet<RoomImage> RoomsImages { get; set; }
        public DbSet<Customer> customers { get; set; }
        public DbSet<Booking> bookings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>()
                .HasOne(r => r.LoaiPhong)
                .WithMany(rt => rt.Rooms)
                .HasForeignKey(r => r.MaLoaiPhong);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Menu> menus { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
    }
}
