using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminDACS.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    MaKH = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.MaKH);
                });

            migrationBuilder.CreateTable(
                name: "RoomsTypes",
                columns: table => new
                {
                    MaLoaiPhong = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LoaiPhong = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomsTypes", x => x.MaLoaiPhong);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    MaNL = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenNL = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SL = table.Column<int>(type: "int", nullable: false),
                    MaMon = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.MaNL);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    MaPhong = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TenPhong = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SucChua = table.Column<int>(type: "int", nullable: false),
                    TinhTrang = table.Column<bool>(type: "bit", nullable: true),
                    MaLoaiPhong = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.MaPhong);
                    table.ForeignKey(
                        name: "FK_Rooms_RoomsTypes_MaLoaiPhong",
                        column: x => x.MaLoaiPhong,
                        principalTable: "RoomsTypes",
                        principalColumn: "MaLoaiPhong",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "menus",
                columns: table => new
                {
                    MaMon = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TenMon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MoTa = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Gia = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LoaiMon = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MaNL = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_menus", x => x.MaMon);
                    table.ForeignKey(
                        name: "FK_menus_Warehouses_MaNL",
                        column: x => x.MaNL,
                        principalTable: "Warehouses",
                        principalColumn: "MaNL");
                });

            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    MaBooking = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaKH = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MaPhong = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BookingTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bookings", x => x.MaBooking);
                    table.ForeignKey(
                        name: "FK_bookings_Rooms_MaPhong",
                        column: x => x.MaPhong,
                        principalTable: "Rooms",
                        principalColumn: "MaPhong",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_bookings_customers_MaKH",
                        column: x => x.MaKH,
                        principalTable: "customers",
                        principalColumn: "MaKH",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoomsImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaPhong = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomsImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomsImages_Rooms_MaPhong",
                        column: x => x.MaPhong,
                        principalTable: "Rooms",
                        principalColumn: "MaPhong",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_bookings_MaKH",
                table: "bookings",
                column: "MaKH");

            migrationBuilder.CreateIndex(
                name: "IX_bookings_MaPhong",
                table: "bookings",
                column: "MaPhong");

            migrationBuilder.CreateIndex(
                name: "IX_menus_MaNL",
                table: "menus",
                column: "MaNL",
                unique: true,
                filter: "[MaNL] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_MaLoaiPhong",
                table: "Rooms",
                column: "MaLoaiPhong");

            migrationBuilder.CreateIndex(
                name: "IX_RoomsImages_MaPhong",
                table: "RoomsImages",
                column: "MaPhong");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bookings");

            migrationBuilder.DropTable(
                name: "menus");

            migrationBuilder.DropTable(
                name: "RoomsImages");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "RoomsTypes");
        }
    }
}
