using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NguyenTienPhat_Final_1638.Models;
using NguyenTienPhat_Final_1638.Repositories;

namespace NguyenTienPhat_Final_1638.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<IActionResult> Index(string? status)
        {
            IEnumerable<Order> orders;

            if (!string.IsNullOrEmpty(status))
            {
                orders = await _orderRepository.GetByStatusAsync(status);
                ViewBag.CurrentStatus = status;
            }
            else
            {
                orders = await _orderRepository.GetAllAsync();
            }

            // Pass available statuses to view
            ViewBag.Statuses = new List<string>
            {
                "Pending",
                "Processing",
                "Shipped",
                "Delivered",
                "Cancelled"
            };

            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            // Pass available statuses to view
            ViewBag.Statuses = new List<string>
            {
                "Pending",
                "Processing",
                "Shipped",
                "Delivered",
                "Cancelled"
            };

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            order.Status = status;
            await _orderRepository.UpdateAsync(order);
            TempData["Success"] = "Order status updated successfully";

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
