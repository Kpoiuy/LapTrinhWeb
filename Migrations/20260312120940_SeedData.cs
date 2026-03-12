using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookDB.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Văn học" },
                    { 2, "Khoa học" },
                    { 3, "Lịch sử" },
                    { 4, "Kinh tế" },
                    { 5, "Công nghệ" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "CategoryId", "ImageUrl", "Price", "Publisher", "Rating", "Title", "Year" },
                values: new object[,]
                {
                    { 1, "Nguyễn Nhật Ánh", 1, "https://salt.tikicdn.com/cache/w1200/ts/product/5e/18/24/2a6154ba08df6ce6161c13f4303fa19e.jpg", 61800m, "NXB Trẻ", 4.8m, "Cho Tôi Xin Một Vé Đi Tuổi Thơ", 2018 },
                    { 2, "Yuval Noah Harari", 3, "https://salt.tikicdn.com/cache/w1200/ts/product/5e/18/24/2a6154ba08df6ce6161c13f4303fa19e.jpg", 189000m, "NXB Văn học", 4.9m, "Sapiens: Lược Sử Loài Người", 2018 },
                    { 3, "Dale Carnegie", 4, "https://salt.tikicdn.com/cache/w1200/ts/product/5e/18/24/2a6154ba08df6ce6161c13f4303fa19e.jpg", 86000m, "NXB Tổng hợp TP.HCM", 4.7m, "Đắc Nhân Tâm", 2020 },
                    { 4, "Halliday & Resnick", 2, "https://salt.tikicdn.com/cache/w1200/ts/product/5e/18/24/2a6154ba08df6ce6161c13f4303fa19e.jpg", 250000m, "NXB Giáo dục", 4.5m, "Vật Lý Đại Cương", 2019 },
                    { 5, "Robert C. Martin", 5, "https://salt.tikicdn.com/cache/w1200/ts/product/5e/18/24/2a6154ba08df6ce6161c13f4303fa19e.jpg", 320000m, "Prentice Hall", 4.9m, "Clean Code", 2021 },
                    { 6, "Nguyễn Nhật Ánh", 1, "https://salt.tikicdn.com/cache/w1200/ts/product/5e/18/24/2a6154ba08df6ce6161c13f4303fa19e.jpg", 75000m, "NXB Trẻ", 4.8m, "Tôi Thấy Hoa Vàng Trên Cỏ Xanh", 2017 },
                    { 7, "Trần Trọng Kim", 3, "https://salt.tikicdn.com/cache/w1200/ts/product/5e/18/24/2a6154ba08df6ce6161c13f4303fa19e.jpg", 150000m, "NXB Văn học", 4.6m, "Lịch Sử Việt Nam", 2016 },
                    { 8, "N. Gregory Mankiw", 4, "https://salt.tikicdn.com/cache/w1200/ts/product/5e/18/24/2a6154ba08df6ce6161c13f4303fa19e.jpg", 180000m, "NXB Thống kê", 4.4m, "Kinh Tế Học Vi Mô", 2020 },
                    { 9, "Thomas H. Cormen", 5, "https://salt.tikicdn.com/cache/w1200/ts/product/5e/18/24/2a6154ba08df6ce6161c13f4303fa19e.jpg", 450000m, "MIT Press", 5.0m, "Introduction to Algorithms", 2022 },
                    { 10, "Paula Yurkanis Bruice", 2, "https://salt.tikicdn.com/cache/w1200/ts/product/5e/18/24/2a6154ba08df6ce6161c13f4303fa19e.jpg", 280000m, "NXB Giáo dục", 4.3m, "Hóa Học Hữu Cơ", 2019 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
