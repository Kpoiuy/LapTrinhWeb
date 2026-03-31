using BookAPI.Data;
using BookAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookAPI.Services;

public static class BookRuntimeSeeder
{
    public static void EnsureSeedData(BookDbContext db)
    {
        var categories = new[]
        {
            new Category { Id = 1, Name = "Lập trình" },
            new Category { Id = 2, Name = "Cơ sở dữ liệu" },
            new Category { Id = 3, Name = "Mạng máy tính" },
            new Category { Id = 4, Name = "Kiến trúc phần mềm" },
            new Category { Id = 5, Name = "Kiểm thử" },
            new Category { Id = 6, Name = "Khoa học dữ liệu" },
            new Category { Id = 7, Name = "Trí tuệ nhân tạo" },
            new Category { Id = 8, Name = "Bảo mật" }
        };

        foreach (var c in categories)
        {
            var old = db.Categories.AsNoTracking().FirstOrDefault(x => x.Id == c.Id);
            if (old is null)
            {
                db.Categories.Add(c);
            }
            else if (old.Name != c.Name)
            {
                var tracked = db.Categories.First(x => x.Id == c.Id);
                tracked.Name = c.Name;
            }
        }
        db.SaveChanges();

        var books = new[]
        {
            NewBook(1, "Clean Code: A Handbook of Agile Software Craftsmanship", "Robert C. Martin", 2008, "Prentice Hall", 450000m, 1, "https://m.media-amazon.com/images/I/51E2055ZGUL._SY445_SX342_.jpg"),
            NewBook(2, "The Pragmatic Programmer: Your Journey To Mastery", "David Thomas, Andrew Hunt", 2019, "Addison-Wesley", 520000m, 1, "https://m.media-amazon.com/images/I/51cUVaBWZzL._SY445_SX342_.jpg"),
            NewBook(3, "Design Patterns: Elements of Reusable Object-Oriented Software", "Erich Gamma, Richard Helm", 1994, "Addison-Wesley", 580000m, 1, "https://m.media-amazon.com/images/I/51szD9HC9pL._SY445_SX342_.jpg"),
            NewBook(4, "Introduction to Algorithms", "Thomas H. Cormen", 2022, "MIT Press", 890000m, 1, "https://m.media-amazon.com/images/I/61Pgdn8Ys-L._SY466_.jpg"),
            NewBook(5, "Database System Concepts", "Abraham Silberschatz", 2019, "McGraw-Hill", 750000m, 2, "https://m.media-amazon.com/images/I/51-9W+zLC+L._SY445_SX342_.jpg"),
            NewBook(6, "SQL Performance Explained", "Markus Winand", 2012, "Self-published", 380000m, 2, "https://sql-performance-explained.com/img/book-cover.png"),
            NewBook(7, "Designing Data-Intensive Applications", "Martin Kleppmann", 2017, "O'Reilly Media", 680000m, 2, "https://m.media-amazon.com/images/I/51ZSpMl1-2L._SY445_SX342_.jpg"),
            NewBook(8, "Computer Networking: A Top-Down Approach", "James Kurose, Keith Ross", 2021, "Pearson", 820000m, 3, "https://m.media-amazon.com/images/I/51xp1%2BoDRML._SY445_SX342_.jpg"),
            NewBook(9, "TCP/IP Illustrated, Volume 1", "W. Richard Stevens", 2011, "Addison-Wesley", 720000m, 3, "https://m.media-amazon.com/images/I/41UhKt8WFNL._SY445_SX342_.jpg"),
            NewBook(10, "Clean Architecture: A Craftsman's Guide", "Robert C. Martin", 2017, "Prentice Hall", 490000m, 4, "https://m.media-amazon.com/images/I/41BKx1AxQWL._SY445_SX342_.jpg"),
            NewBook(11, "Domain-Driven Design: Tackling Complexity", "Eric Evans", 2003, "Addison-Wesley", 650000m, 4, "https://m.media-amazon.com/images/I/51OWGtzQLLL._SY445_SX342_.jpg"),
            NewBook(12, "Building Microservices", "Sam Newman", 2021, "O'Reilly Media", 580000m, 4, "https://m.media-amazon.com/images/I/51SHMB1cfWL._SY445_SX342_.jpg"),
            NewBook(13, "Test Driven Development: By Example", "Kent Beck", 2002, "Addison-Wesley", 420000m, 5, "https://m.media-amazon.com/images/I/51kDbV%2BN65L._SY445_SX342_.jpg"),
            NewBook(14, "Unit Testing Principles, Practices, and Patterns", "Vladimir Khorikov", 2020, "Manning", 530000m, 5, "https://m.media-amazon.com/images/I/41-W+8FxqNL._SY445_SX342_.jpg"),
            NewBook(15, "Python for Data Analysis", "Wes McKinney", 2022, "O'Reilly Media", 620000m, 6, "https://m.media-amazon.com/images/I/51cUNf8zukL._SY445_SX342_.jpg"),
            NewBook(16, "Hands-On Machine Learning with Scikit-Learn, Keras, and TensorFlow", "Aurélien Géron", 2022, "O'Reilly Media", 780000m, 7, "https://m.media-amazon.com/images/I/51aqYc1QyrL._SY445_SX342_.jpg"),
            NewBook(17, "Deep Learning", "Ian Goodfellow, Yoshua Bengio", 2016, "MIT Press", 920000m, 7, "https://m.media-amazon.com/images/I/61fim5QqaqL._SY466_.jpg"),
            NewBook(18, "The Web Application Hacker's Handbook", "Dafydd Stuttard, Marcus Pinto", 2011, "Wiley", 680000m, 8, "https://m.media-amazon.com/images/I/51zyQN8g6LL._SY445_SX342_.jpg"),
            NewBook(19, "Cryptography and Network Security", "William Stallings", 2022, "Pearson", 750000m, 8, "https://m.media-amazon.com/images/I/51Ga3G5DKWL._SY445_SX342_.jpg"),
            NewBook(20, "Refactoring: Improving the Design of Existing Code", "Martin Fowler", 2018, "Addison-Wesley", 560000m, 1, "https://m.media-amazon.com/images/I/51ttgxwzArL._SY445_SX342_.jpg")
        };

        foreach (var book in books)
        {
            var old = db.Books.FirstOrDefault(x => x.Id == book.Id);
            if (old is null)
            {
                db.Books.Add(book);
            }
            else
            {
                old.Title = book.Title;
                old.Author = book.Author;
                old.Year = book.Year;
                old.Publisher = book.Publisher;
                old.Price = book.Price;
                old.CategoryId = book.CategoryId;
                old.ImagePath = book.ImagePath;
            }
        }

        db.SaveChanges();
    }

    private static Book NewBook(int id, string title, string author, int year, string publisher, decimal price, int categoryId, string imagePath)
    {
        return new Book
        {
            Id = id,
            Title = title,
            Author = author,
            Year = year,
            Publisher = publisher,
            Price = price,
            CategoryId = categoryId,
            ImagePath = imagePath
        };
    }
}
