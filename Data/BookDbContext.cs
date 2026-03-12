using BookDB.Models;
using Microsoft.EntityFrameworkCore;

namespace BookDB.Data
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Book entity
            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(b => b.Id);
                entity.Property(b => b.Title).IsRequired().HasMaxLength(200);
                entity.Property(b => b.Author).IsRequired().HasMaxLength(100);
                entity.Property(b => b.Year).IsRequired();
                entity.Property(b => b.Publisher).IsRequired().HasMaxLength(100);
                entity.Property(b => b.ImageUrl).HasMaxLength(500);
                entity.Property(b => b.Price).HasColumnType("decimal(18,2)");
                entity.Property(b => b.Rating).HasColumnType("decimal(3,2)");

                // Configure relationship
                entity.HasOne(b => b.Category)
                      .WithMany(c => c.Books)
                      .HasForeignKey(b => b.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
                entity.HasIndex(c => c.Name).IsUnique();
            });

            // Seed Categories
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Văn học" },
                new Category { Id = 2, Name = "Khoa học" },
                new Category { Id = 3, Name = "Lịch sử" },
                new Category { Id = 4, Name = "Kinh tế" },
                new Category { Id = 5, Name = "Công nghệ" }
            );

            // Seed Books
            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, Title = "Cho Tôi Xin Một Vé Đi Tuổi Thơ", Author = "Nguyễn Nhật Ánh", Year = 2018, Publisher = "NXB Trẻ", CategoryId = 1, ImageUrl = "https://salt.tikicdn.com/cache/w1200/ts/product/5e/18/24/2a6154ba08df6ce6161c13f4303fa19e.jpg", Price = 61800, Rating = 4.8m },
                new Book { Id = 2, Title = "Sapiens: Lược Sử Loài Người", Author = "Yuval Noah Harari", Year = 2018, Publisher = "NXB Văn học", CategoryId = 3, ImageUrl = "https://salt.tikicdn.com/cache/w1200/ts/product/5e/18/24/2a6154ba08df6ce6161c13f4303fa19e.jpg", Price = 189000, Rating = 4.9m },
                new Book { Id = 3, Title = "Đắc Nhân Tâm", Author = "Dale Carnegie", Year = 2020, Publisher = "NXB Tổng hợp TP.HCM", CategoryId = 4, ImageUrl = "https://salt.tikicdn.com/cache/w1200/ts/product/5e/18/24/2a6154ba08df6ce6161c13f4303fa19e.jpg", Price = 86000, Rating = 4.7m },
                new Book { Id = 4, Title = "Vật Lý Đại Cương", Author = "Halliday & Resnick", Year = 2019, Publisher = "NXB Giáo dục", CategoryId = 2, ImageUrl = "https://salt.tikicdn.com/cache/w1200/ts/product/5e/18/24/2a6154ba08df6ce6161c13f4303fa19e.jpg", Price = 250000, Rating = 4.5m },
                new Book { Id = 5, Title = "Clean Code", Author = "Robert C. Martin", Year = 2021, Publisher = "Prentice Hall", CategoryId = 5, ImageUrl = "https://salt.tikicdn.com/cache/w1200/ts/product/5e/18/24/2a6154ba08df6ce6161c13f4303fa19e.jpg", Price = 320000, Rating = 4.9m },
                new Book { Id = 6, Title = "Tôi Thấy Hoa Vàng Trên Cỏ Xanh", Author = "Nguyễn Nhật Ánh", Year = 2017, Publisher = "NXB Trẻ", CategoryId = 1, ImageUrl = "https://salt.tikicdn.com/cache/w1200/ts/product/5e/18/24/2a6154ba08df6ce6161c13f4303fa19e.jpg", Price = 75000, Rating = 4.8m },
                new Book { Id = 7, Title = "Lịch Sử Việt Nam", Author = "Trần Trọng Kim", Year = 2016, Publisher = "NXB Văn học", CategoryId = 3, ImageUrl = "https://salt.tikicdn.com/cache/w1200/ts/product/5e/18/24/2a6154ba08df6ce6161c13f4303fa19e.jpg", Price = 150000, Rating = 4.6m },
                new Book { Id = 8, Title = "Kinh Tế Học Vi Mô", Author = "N. Gregory Mankiw", Year = 2020, Publisher = "NXB Thống kê", CategoryId = 4, ImageUrl = "https://salt.tikicdn.com/cache/w1200/ts/product/5e/18/24/2a6154ba08df6ce6161c13f4303fa19e.jpg", Price = 180000, Rating = 4.4m },
                new Book { Id = 9, Title = "Introduction to Algorithms", Author = "Thomas H. Cormen", Year = 2022, Publisher = "MIT Press", CategoryId = 5, ImageUrl = "https://salt.tikicdn.com/cache/w1200/ts/product/5e/18/24/2a6154ba08df6ce6161c13f4303fa19e.jpg", Price = 450000, Rating = 5.0m },
                new Book { Id = 10, Title = "Hóa Học Hữu Cơ", Author = "Paula Yurkanis Bruice", Year = 2019, Publisher = "NXB Giáo dục", CategoryId = 2, ImageUrl = "https://salt.tikicdn.com/cache/w1200/ts/product/5e/18/24/2a6154ba08df6ce6161c13f4303fa19e.jpg", Price = 280000, Rating = 4.3m }
            );
        }
    }
}
