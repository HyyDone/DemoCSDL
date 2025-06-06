using AdminDACS;
using AdminDACS.Models;
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
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderDetail> ordersDetail { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<ElectricityBill> ElectricityBillRecords { get; set; }
        public DbSet<WaterBill> WaterBillRecords { get; set; }
        public DbSet<MaterialInput> MaterialInputs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<TempOrderItem> TempOrderItems { get; set; }
    }
}
