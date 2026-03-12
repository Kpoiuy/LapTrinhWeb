using Microsoft.AspNetCore.Mvc;

namespace BookDB.Controllers
{
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;

        public CartController(ILogger<CartController> logger)
        {
            _logger = logger;
        }

        // POST: Cart/AddToCart/5
        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            // Placeholder implementation - in a real app, this would add to session or database
            _logger.LogInformation("Book {BookId} added to cart", id);
            TempData["Message"] = "Book added to cart successfully!";
            return RedirectToAction("Index", "Books");
        }
    }
}
