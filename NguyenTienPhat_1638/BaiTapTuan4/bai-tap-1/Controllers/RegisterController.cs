using BaiTap1.Models;
using Microsoft.AspNetCore.Mvc;

namespace BaiTap1.Controllers;

public class RegisterController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View(new User());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(User user)
    {
        if (!ModelState.IsValid)
        {
            return View(user);
        }

        return View("Success", user);
    }
}
