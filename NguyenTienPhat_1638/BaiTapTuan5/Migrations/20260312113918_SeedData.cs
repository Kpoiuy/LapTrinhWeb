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
                    { 1, "Fiction" },
                    { 2, "Science" },
                    { 3, "History" },
                    { 4, "Technology" },
                    { 5, "Biography" }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "CategoryId", "ImageUrl", "Price", "Publisher", "Rating", "Title", "Year" },
                values: new object[,]
                {
                    { 1, "F. Scott Fitzgerald", 1, "https://covers.openlibrary.org/b/id/7222246-L.jpg", 12.99m, "Scribner", 4.5m, "The Great Gatsby", 1925 },
                    { 2, "Harper Lee", 1, "https://covers.openlibrary.org/b/id/8228691-L.jpg", 14.99m, "J.B. Lippincott & Co.", 4.8m, "To Kill a Mockingbird", 1960 },
                    { 3, "George Orwell", 1, "https://covers.openlibrary.org/b/id/7222246-L.jpg", 13.99m, "Secker & Warburg", 4.7m, "1984", 1949 },
                    { 4, "Jane Austen", 1, null, 11.99m, "T. Egerton", 4.6m, "Pride and Prejudice", 1813 },
                    { 5, "Stephen Hawking", 2, "https://covers.openlibrary.org/b/id/9255566-L.jpg", 18.99m, "Bantam Books", 4.4m, "A Brief History of Time", 1988 },
                    { 6, "Richard Dawkins", 2, "https://covers.openlibrary.org/b/id/8231986-L.jpg", 16.99m, "Oxford University Press", 4.3m, "The Selfish Gene", 1976 },
                    { 7, "Carl Sagan", 2, null, 19.99m, "Random House", 4.9m, "Cosmos", 1980 },
                    { 8, "Yuval Noah Harari", 3, "https://covers.openlibrary.org/b/id/8231986-L.jpg", 22.99m, "Harper", 4.7m, "Sapiens: A Brief History of Humankind", 2011 },
                    { 9, "Jared Diamond", 3, null, 20.99m, "W. W. Norton & Company", 4.5m, "Guns, Germs, and Steel", 1997 },
                    { 10, "Peter Frankopan", 3, "https://covers.openlibrary.org/b/id/8231986-L.jpg", 21.99m, "Bloomsbury", 4.4m, "The Silk Roads", 2015 },
                    { 11, "Walter Isaacson", 4, "https://covers.openlibrary.org/b/id/8231986-L.jpg", 24.99m, "Simon & Schuster", 4.6m, "The Innovators", 2014 },
                    { 12, "Robert C. Martin", 4, null, 44.99m, "Prentice Hall", 4.8m, "Clean Code", 2008 },
                    { 13, "Andrew Hunt and David Thomas", 4, "https://covers.openlibrary.org/b/id/8231986-L.jpg", 39.99m, "Addison-Wesley", 4.7m, "The Pragmatic Programmer", 1999 },
                    { 14, "Walter Isaacson", 5, "https://covers.openlibrary.org/b/id/8231986-L.jpg", 19.99m, "Simon & Schuster", 4.5m, "Steve Jobs", 2011 },
                    { 15, "Anne Frank", 5, null, 12.99m, "Contact Publishing", 4.9m, "The Diary of a Young Girl", 1947 },
                    { 16, "Nelson Mandela", 5, "https://covers.openlibrary.org/b/id/8231986-L.jpg", 17.99m, "Little, Brown and Company", 4.8m, "Long Walk to Freedom", 1994 }
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
                table: "Books",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 16);

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
