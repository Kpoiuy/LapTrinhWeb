using Microsoft.EntityFrameworkCore;
using BookDB.Models;

namespace BookDB.Data
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Book entity
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Author).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Year).IsRequired();
                entity.Property(e => e.Publisher).IsRequired().HasMaxLength(100);
                entity.Property(e => e.ImageUrl).HasMaxLength(500);
                entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Rating).HasColumnType("decimal(3,2)");

                // Configure relationship with Category
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Books)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Fiction" },
                new Category { Id = 2, Name = "Science" },
                new Category { Id = 3, Name = "History" },
                new Category { Id = 4, Name = "Technology" },
                new Category { Id = 5, Name = "Biography" }
            );

            // Seed Books
            modelBuilder.Entity<Book>().HasData(
                // Fiction books
                new Book
                {
                    Id = 1,
                    Title = "The Great Gatsby",
                    Author = "F. Scott Fitzgerald",
                    Year = 1925,
                    Publisher = "Scribner",
                    CategoryId = 1,
                    ImageUrl = "https://covers.openlibrary.org/b/id/7222246-L.jpg",
                    Price = 12.99m,
                    Rating = 4.5m
                },
                new Book
                {
                    Id = 2,
                    Title = "To Kill a Mockingbird",
                    Author = "Harper Lee",
                    Year = 1960,
                    Publisher = "J.B. Lippincott & Co.",
                    CategoryId = 1,
                    ImageUrl = "https://covers.openlibrary.org/b/id/8228691-L.jpg",
                    Price = 14.99m,
                    Rating = 4.8m
                },
                new Book
                {
                    Id = 3,
                    Title = "1984",
                    Author = "George Orwell",
                    Year = 1949,
                    Publisher = "Secker & Warburg",
                    CategoryId = 1,
                    ImageUrl = "https://covers.openlibrary.org/b/id/7222246-L.jpg",
                    Price = 13.99m,
                    Rating = 4.7m
                },
                new Book
                {
                    Id = 4,
                    Title = "Pride and Prejudice",
                    Author = "Jane Austen",
                    Year = 1813,
                    Publisher = "T. Egerton",
                    CategoryId = 1,
                    Price = 11.99m,
                    Rating = 4.6m
                },
                // Science books
                new Book
                {
                    Id = 5,
                    Title = "A Brief History of Time",
                    Author = "Stephen Hawking",
                    Year = 1988,
                    Publisher = "Bantam Books",
                    CategoryId = 2,
                    ImageUrl = "https://covers.openlibrary.org/b/id/9255566-L.jpg",
                    Price = 18.99m,
                    Rating = 4.4m
                },
                new Book
                {
                    Id = 6,
                    Title = "The Selfish Gene",
                    Author = "Richard Dawkins",
                    Year = 1976,
                    Publisher = "Oxford University Press",
                    CategoryId = 2,
                    ImageUrl = "https://covers.openlibrary.org/b/id/8231986-L.jpg",
                    Price = 16.99m,
                    Rating = 4.3m
                },
                new Book
                {
                    Id = 7,
                    Title = "Cosmos",
                    Author = "Carl Sagan",
                    Year = 1980,
                    Publisher = "Random House",
                    CategoryId = 2,
                    Price = 19.99m,
                    Rating = 4.9m
                },
                // History books
                new Book
                {
                    Id = 8,
                    Title = "Sapiens: A Brief History of Humankind",
                    Author = "Yuval Noah Harari",
                    Year = 2011,
                    Publisher = "Harper",
                    CategoryId = 3,
                    ImageUrl = "https://covers.openlibrary.org/b/id/8231986-L.jpg",
                    Price = 22.99m,
                    Rating = 4.7m
                },
                new Book
                {
                    Id = 9,
                    Title = "Guns, Germs, and Steel",
                    Author = "Jared Diamond",
                    Year = 1997,
                    Publisher = "W. W. Norton & Company",
                    CategoryId = 3,
                    Price = 20.99m,
                    Rating = 4.5m
                },
                new Book
                {
                    Id = 10,
                    Title = "The Silk Roads",
                    Author = "Peter Frankopan",
                    Year = 2015,
                    Publisher = "Bloomsbury",
                    CategoryId = 3,
                    ImageUrl = "https://covers.openlibrary.org/b/id/8231986-L.jpg",
                    Price = 21.99m,
                    Rating = 4.4m
                },
                // Technology books
                new Book
                {
                    Id = 11,
                    Title = "The Innovators",
                    Author = "Walter Isaacson",
                    Year = 2014,
                    Publisher = "Simon & Schuster",
                    CategoryId = 4,
                    ImageUrl = "https://covers.openlibrary.org/b/id/8231986-L.jpg",
                    Price = 24.99m,
                    Rating = 4.6m
                },
                new Book
                {
                    Id = 12,
                    Title = "Clean Code",
                    Author = "Robert C. Martin",
                    Year = 2008,
                    Publisher = "Prentice Hall",
                    CategoryId = 4,
                    Price = 44.99m,
                    Rating = 4.8m
                },
                new Book
                {
                    Id = 13,
                    Title = "The Pragmatic Programmer",
                    Author = "Andrew Hunt and David Thomas",
                    Year = 1999,
                    Publisher = "Addison-Wesley",
                    CategoryId = 4,
                    ImageUrl = "https://covers.openlibrary.org/b/id/8231986-L.jpg",
                    Price = 39.99m,
                    Rating = 4.7m
                },
                // Biography books
                new Book
                {
                    Id = 14,
                    Title = "Steve Jobs",
                    Author = "Walter Isaacson",
                    Year = 2011,
                    Publisher = "Simon & Schuster",
                    CategoryId = 5,
                    ImageUrl = "https://covers.openlibrary.org/b/id/8231986-L.jpg",
                    Price = 19.99m,
                    Rating = 4.5m
                },
                new Book
                {
                    Id = 15,
                    Title = "The Diary of a Young Girl",
                    Author = "Anne Frank",
                    Year = 1947,
                    Publisher = "Contact Publishing",
                    CategoryId = 5,
                    Price = 12.99m,
                    Rating = 4.9m
                },
                new Book
                {
                    Id = 16,
                    Title = "Long Walk to Freedom",
                    Author = "Nelson Mandela",
                    Year = 1994,
                    Publisher = "Little, Brown and Company",
                    CategoryId = 5,
                    ImageUrl = "https://covers.openlibrary.org/b/id/8231986-L.jpg",
                    Price = 17.99m,
                    Rating = 4.8m
                }
            );
        }
    }
}
