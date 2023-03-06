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
        return View();
    }

    [HttpPost]
    public IActionResult SignIn(string email, string password)
    {
        try
        {
            var user = _logInService.LogIn(email, password);
            HttpContext.Session.SetInt32("UserId", user.ID);
            return RedirectToAction("Index", "Account");
        }
        catch (Exception e)
        {
            Debug.WriteLine("ERROR:" + e.Message);
            return RedirectToAction("Index", "Home");
        }
    }
}
