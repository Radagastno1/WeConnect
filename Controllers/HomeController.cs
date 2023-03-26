using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeConnect.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;

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

    // [HttpPost]
    // [AllowAnonymous]
    // public async Task<IActionResult> SignIn(string email, string password)
    // {
    //     try
    //     {
    //         var user = await _logInService.LogIn(email, password);

    //         if (user == null)
    //         {
    //             return BadRequest("Invalid email or password.");
    //         }

    //        //hämtar jwt token med det unika id:t
    //         var jwtSecurityToken = _logInService.GenerateJwtToken(user.ID);


    //         Response.Headers.Add("Authorization", "Bearer " + jwtSecurityToken);
    //         // return RedirectToAction("Index", "Account");
    //         return Ok();
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.WriteLine("error:" + e.Message);
    //         return RedirectToAction("Index", "Home");
    //     }
    // }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> SignIn(string email, string password)
    {
        try
        {
            var user = await _logInService.LogIn(email, password);

            if (user == null)
            {
                return BadRequest("Invalid email or password.");
            }

            var jwtSecurityToken = _logInService.GenerateJwtToken(user.ID);

            // Lägg till Authorization-header med JWT-token i Request
            var request = new HttpRequestMessage(HttpMethod.Get, "https://localhost:7095/Account/Index");
            request.Headers.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                jwtSecurityToken
            );

            HttpClient client = new();
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Failed to log in.");
            }

            return View("Index");
        }
        catch (Exception e)
        {
            Debug.WriteLine("error:" + e.Message);
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
