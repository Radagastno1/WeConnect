using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeConnect.Models;

namespace WeConnect.Controllers;

public class AccountController : Controller
{
    private readonly LogInService _logInService;
    private readonly UserService _userService;

    public AccountController(LogInService logInService, UserService userService)
    {
        _logInService = logInService;
        _userService = userService;
    }

    public IActionResult Index()
    {
        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = _userService.GetUserById(userId);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var homePageViewModel = new HomePageViewModel
            {
                Id = user.ID,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            };
            return View(homePageViewModel);
        }
        catch (Exception e)
        {
            Debug.WriteLine("ERROR:" + e.Message);
            return RedirectToAction("Index", "Home");
        }
    }
}
