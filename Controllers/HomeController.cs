using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeConnect.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WeConnect.Controllers;

public class HomeController : Controller
{
    private readonly LogInService _logInService;
    private readonly UserService _userService;
    private readonly IConfiguration _configuration;

    public HomeController(
        LogInService logInService,
        UserService userService,
        IConfiguration configuration
    )
    {
        _logInService = logInService;
        _userService = userService;
        _configuration = configuration;
    }

    [Route("/")]
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
    [AllowAnonymous]
    // [ValidateAntiForgeryToken]
    public async Task<IActionResult> SignIn(string email, string password)
    {
        try
        {
            var user = await _logInService.LogIn(email, password);

            if (user == null)
            {
                return BadRequest("Invalid email or password.");
            }

            // Get JWT token for the user
            var jwtSecurityToken = _logInService.GenerateJwtToken(user.ID, "Member");

            // Add the token to an HttpOnly cookie
            var options = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("your-cookie-name", jwtSecurityToken, options);

            return RedirectToAction("Index", "Account", new { token = jwtSecurityToken });
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
