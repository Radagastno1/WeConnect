using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeConnect.Models;

namespace WeConnect.Controllers;

public class HomeController : Controller
{
    private readonly LogInService _logInService;
    private readonly UserService _userService;

    public HomeController(LogInService logInService, UserService userService)
    {
        _logInService = logInService;
        _userService = userService;
    }

    public IActionResult Index()
    {
        // Kolla om cookien redan finns
        bool cookieExists = HttpContext.Request.Cookies.ContainsKey("TestCookie");
        if (!cookieExists)
        {
            // Om cookien inte finns, visa popup-rutan
            ViewBag.ShowCookiePopup = true;

            //Sätt cookien
        HttpContext.Response.Cookies.Append("TestCookie", "12345");
        }
        else
        {
            // Om cookien finns, dölj popup-rutan
            ViewBag.ShowCookiePopup = false;
        }
        return View();
    }

    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(string email, string password)
    {
        try
        {
            var user = await _logInService.LogIn(email, password);
            HttpContext.Session.SetInt32("UserId", user.ID);
            return RedirectToAction("Index", "Account");
        }
        catch (Exception e)
        {
            Debug.WriteLine("ERROR:" + e.Message);
            return RedirectToAction("Index", "Home");
        }
    }

    public IActionResult SignUp()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }
}
