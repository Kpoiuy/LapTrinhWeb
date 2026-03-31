using Microsoft.AspNetCore.Identity;
using NguyenTienPhat_Final_1638.Models;

namespace NguyenTienPhat_Final_1638.DataAccess
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(
            IServiceProvider serviceProvider,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            // Create roles
            await CreateRolesAsync(roleManager);

            // Create admin user
            await CreateAdminUserAsync(userManager);

            // Seed sample data
            await SeedSampleDataAsync(serviceProvider);
        }

        private static async Task CreateRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { SD.Role_Admin, SD.Role_Customer, SD.Role_Employee, SD.Role_Company };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        private static async Task CreateAdminUserAsync(UserManager<ApplicationUser> userManager)
        {
            string adminEmail = "admin@shop.com";
            string adminPassword = "Admin@123";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Administrator",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, SD.Role_Admin);
                }
            }
        }

        private static async Task SeedSampleDataAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Seed Categories
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Electronics" },
                    new Category { Name = "Clothing" },
                    new Category { Name = "Books" },
                    new Category { Name = "Home & Garden" },
                    new Category { Name = "Sports & Outdoors" },
                    new Category { Name = "Toys & Games" }
                };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            // Seed Products - 32 products with placeholder images
            if (!context.Products.Any())
            {
                var electronics = context.Categories.First(c => c.Name == "Electronics");
                var clothing = context.Categories.First(c => c.Name == "Clothing");
                var books = context.Categories.First(c => c.Name == "Books");
                var homeGarden = context.Categories.First(c => c.Name == "Home & Garden");
                var sports = context.Categories.First(c => c.Name == "Sports & Outdoors");
                var toys = context.Categories.First(c => c.Name == "Toys & Games");

                var products = new List<Product>
                {
                    // Electronics (10 products)
                    new Product { Name = "MacBook Pro 16\"", Price = 2499.99m, Description = "Powerful laptop with M3 Pro chip, 16GB RAM, 512GB SSD", CategoryId = electronics.Id, ImageUrl = "https://via.placeholder.com/400x400/1e3a8a/ffffff?text=MacBook+Pro" },
                    new Product { Name = "iPhone 15 Pro Max", Price = 1199.99m, Description = "Latest flagship smartphone with A17 Pro chip", CategoryId = electronics.Id, ImageUrl = "https://via.placeholder.com/400x400/000000/ffffff?text=iPhone+15" },
                    new Product { Name = "Samsung Galaxy S24 Ultra", Price = 1299.99m, Description = "Premium Android phone with S Pen and 200MP camera", CategoryId = electronics.Id, ImageUrl = "https://via.placeholder.com/400x400/1a1a1a/ffffff?text=Galaxy+S24" },
                    new Product { Name = "iPad Air M2", Price = 599.99m, Description = "Versatile tablet with M2 chip and 11-inch display", CategoryId = electronics.Id, ImageUrl = "https://via.placeholder.com/400x400/4a5568/ffffff?text=iPad+Air" },
                    new Product { Name = "Sony WH-1000XM5", Price = 399.99m, Description = "Industry-leading noise canceling headphones", CategoryId = electronics.Id, ImageUrl = "https://via.placeholder.com/400x400/2d3748/ffffff?text=Sony+Headphones" },
                    new Product { Name = "Dell XPS 15", Price = 1899.99m, Description = "High-performance Windows laptop with RTX 4050", CategoryId = electronics.Id, ImageUrl = "https://via.placeholder.com/400x400/0066cc/ffffff?text=Dell+XPS" },
                    new Product { Name = "Apple Watch Series 9", Price = 429.99m, Description = "Advanced health and fitness smartwatch", CategoryId = electronics.Id, ImageUrl = "https://via.placeholder.com/400x400/1e293b/ffffff?text=Apple+Watch" },
                    new Product { Name = "Canon EOS R6 Mark II", Price = 2499.99m, Description = "Professional mirrorless camera with 4K 60fps", CategoryId = electronics.Id, ImageUrl = "https://via.placeholder.com/400x400/dc2626/ffffff?text=Canon+Camera" },
                    new Product { Name = "Nintendo Switch OLED", Price = 349.99m, Description = "Hybrid gaming console with 7-inch OLED screen", CategoryId = electronics.Id, ImageUrl = "https://via.placeholder.com/400x400/e60012/ffffff?text=Switch+OLED" },
                    new Product { Name = "Bose SoundLink Flex", Price = 149.99m, Description = "Portable waterproof Bluetooth speaker", CategoryId = electronics.Id, ImageUrl = "https://via.placeholder.com/400x400/000000/ffffff?text=Bose+Speaker" },

                    // Clothing (10 products)
                    new Product { Name = "Levi's 501 Original Jeans", Price = 89.99m, Description = "Classic straight-fit denim jeans", CategoryId = clothing.Id, ImageUrl = "https://via.placeholder.com/400x400/003f87/ffffff?text=Levis+Jeans" },
                    new Product { Name = "Nike Air Max 270", Price = 159.99m, Description = "Comfortable lifestyle sneakers with Max Air", CategoryId = clothing.Id, ImageUrl = "https://via.placeholder.com/400x400/ff6b00/ffffff?text=Nike+Shoes" },
                    new Product { Name = "Adidas Essentials Hoodie", Price = 69.99m, Description = "Warm fleece hoodie for casual wear", CategoryId = clothing.Id, ImageUrl = "https://via.placeholder.com/400x400/000000/ffffff?text=Adidas+Hoodie" },
                    new Product { Name = "Ralph Lauren Polo Shirt", Price = 89.99m, Description = "Classic fit polo shirt in cotton piqué", CategoryId = clothing.Id, ImageUrl = "https://via.placeholder.com/400x400/002c77/ffffff?text=Polo+Shirt" },
                    new Product { Name = "The North Face Jacket", Price = 249.99m, Description = "Waterproof outdoor jacket with hood", CategoryId = clothing.Id, ImageUrl = "https://via.placeholder.com/400x400/000000/ffffff?text=TNF+Jacket" },
                    new Product { Name = "Ray-Ban Aviator Classic", Price = 159.99m, Description = "Iconic aviator sunglasses with UV protection", CategoryId = clothing.Id, ImageUrl = "https://via.placeholder.com/400x400/c4a000/000000?text=Ray-Ban" },
                    new Product { Name = "Timberland 6-Inch Boots", Price = 199.99m, Description = "Premium waterproof leather boots", CategoryId = clothing.Id, ImageUrl = "https://via.placeholder.com/400x400/c19a6b/000000?text=Timberland" },
                    new Product { Name = "Patagonia Better Sweater", Price = 139.99m, Description = "Eco-friendly fleece jacket", CategoryId = clothing.Id, ImageUrl = "https://via.placeholder.com/400x400/006341/ffffff?text=Patagonia" },
                    new Product { Name = "Under Armour Tech T-Shirt", Price = 29.99m, Description = "Moisture-wicking performance tee", CategoryId = clothing.Id, ImageUrl = "https://via.placeholder.com/400x400/000000/ffffff?text=UA+Tee" },
                    new Product { Name = "Vans Old Skool Sneakers", Price = 69.99m, Description = "Classic skate shoes with side stripe", CategoryId = clothing.Id, ImageUrl = "https://via.placeholder.com/400x400/000000/ffffff?text=Vans+Shoes" },

                    // Books (4 products)
                    new Product { Name = "The Great Gatsby", Price = 14.99m, Description = "F. Scott Fitzgerald's masterpiece", CategoryId = books.Id, ImageUrl = "https://via.placeholder.com/400x400/1e3a8a/ffffff?text=Great+Gatsby" },
                    new Product { Name = "1984 by George Orwell", Price = 16.99m, Description = "Dystopian novel about totalitarianism", CategoryId = books.Id, ImageUrl = "https://via.placeholder.com/400x400/7c2d12/ffffff?text=1984" },
                    new Product { Name = "Harry Potter Complete Set", Price = 129.99m, Description = "All 7 books in the magical series", CategoryId = books.Id, ImageUrl = "https://via.placeholder.com/400x400/740001/ffffff?text=Harry+Potter" },
                    new Product { Name = "Atomic Habits", Price = 24.99m, Description = "Guide to building good habits", CategoryId = books.Id, ImageUrl = "https://via.placeholder.com/400x400/059669/ffffff?text=Atomic+Habits" },

                    // Home & Garden (4 products)
                    new Product { Name = "Dyson V15 Detect", Price = 749.99m, Description = "Powerful cordless vacuum with laser", CategoryId = homeGarden.Id, ImageUrl = "https://via.placeholder.com/400x400/6b21a8/ffffff?text=Dyson+Vacuum" },
                    new Product { Name = "KitchenAid Stand Mixer", Price = 449.99m, Description = "Professional 5-quart stand mixer", CategoryId = homeGarden.Id, ImageUrl = "https://via.placeholder.com/400x400/dc2626/ffffff?text=KitchenAid" },
                    new Product { Name = "Nespresso Vertuo Next", Price = 179.99m, Description = "Premium coffee and espresso maker", CategoryId = homeGarden.Id, ImageUrl = "https://via.placeholder.com/400x400/4a5568/ffffff?text=Nespresso" },
                    new Product { Name = "Philips XXL Air Fryer", Price = 199.99m, Description = "Large capacity air fryer for healthy cooking", CategoryId = homeGarden.Id, ImageUrl = "https://via.placeholder.com/400x400/0066cc/ffffff?text=Air+Fryer" },

                    // Sports & Outdoors (2 products)
                    new Product { Name = "Manduka PRO Yoga Mat", Price = 119.99m, Description = "Professional-grade non-slip yoga mat", CategoryId = sports.Id, ImageUrl = "https://via.placeholder.com/400x400/7c3aed/ffffff?text=Yoga+Mat" },
                    new Product { Name = "Coleman Sundome Tent", Price = 89.99m, Description = "4-person dome tent with WeatherTec", CategoryId = sports.Id, ImageUrl = "https://via.placeholder.com/400x400/16a34a/ffffff?text=Camping+Tent" },

                    // Toys & Games (2 products)
                    new Product { Name = "LEGO Millennium Falcon", Price = 169.99m, Description = "Star Wars building set with 1,351 pieces", CategoryId = toys.Id, ImageUrl = "https://via.placeholder.com/400x400/fbbf24/000000?text=LEGO" },
                    new Product { Name = "Monopoly Classic Edition", Price = 24.99m, Description = "Classic property trading board game", CategoryId = toys.Id, ImageUrl = "https://via.placeholder.com/400x400/dc2626/ffffff?text=Monopoly" }
                };

                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }

            // Seed Sample Customers
            var customerEmails = new[] { "customer1@test.com", "customer2@test.com", "customer3@test.com" };
            var customers = new List<ApplicationUser>();

            foreach (var email in customerEmails)
            {
                var existingUser = await userManager.FindByEmailAsync(email);
                if (existingUser == null)
                {
                    var customer = new ApplicationUser
                    {
                        UserName = email,
                        Email = email,
                        FullName = $"Customer {email[8]}",
                        Address = $"{100 + int.Parse(email[8].ToString())} Main Street, City",
                        Age = (20 + int.Parse(email[8].ToString()) * 5).ToString(),
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(customer, "Customer@123");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(customer, SD.Role_Customer);
                        customers.Add(customer);
                    }
                }
                else
                {
                    customers.Add(existingUser);
                }
            }

            // Seed Sample Orders
            if (!context.Orders.Any() && customers.Any())
            {
                var products = context.Products.ToList();
                var random = new Random();
                var statuses = new[] { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };

                var orders = new List<Order>();

                for (int i = 0; i < 15; i++)
                {
                    var customer = customers[i % customers.Count];
                    var orderDate = DateTime.Now.AddDays(-random.Next(1, 60));
                    var itemCount = random.Next(1, 5);
                    var orderItems = new List<OrderItem>();
                    decimal totalAmount = 0;

                    for (int j = 0; j < itemCount; j++)
                    {
                        var product = products[random.Next(products.Count)];
                        var quantity = random.Next(1, 4);
                        var price = product.Price;

                        orderItems.Add(new OrderItem
                        {
                            ProductId = product.Id,
                            Quantity = quantity,
                            Price = price
                        });

                        totalAmount += price * quantity;
                    }

                    orders.Add(new Order
                    {
                        UserId = customer.Id,
                        OrderDate = orderDate,
                        TotalAmount = totalAmount,
                        Status = statuses[i % statuses.Length],
                        ShippingAddress = customer.Address ?? "123 Default Street",
                        OrderItems = orderItems
                    });
                }

                context.Orders.AddRange(orders);
                await context.SaveChangesAsync();
            }
        }
    }
}
