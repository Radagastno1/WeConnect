using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeConnect.Models;
using WeConnect.ViewModels;

namespace WeConnect.Controllers;

public class FriendRequestController : Controller
{
    private readonly FriendService _friendService;
    private readonly UserService _userService;

    public FriendRequestController(FriendService friendService, UserService userService)
    {
        _friendService = friendService;
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = await _userService.GetUserById(userId);
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
    [HttpPost]
    public async Task<IActionResult> AddFriend(int id)
    {
        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = await _userService.GetUserById(userId);
            _friendService.Create(user, id);

            // HttpContext.Session.SetInt32("FriendId", id);
            // var friendId = HttpContext.Session.GetInt32("FriendId");
            return RedirectToAction("ShowProfile", "Profile", new{id = id});
        }
        catch
        {
            return RedirectToAction("Error", "Profile");
        }
    }

    private List<FriendViewModel> FriendsToViewModel(List<User> users)
    {
        return users.Select(u => UserToFriendViewModel(u)).ToList();
    }

    private FriendViewModel UserToFriendViewModel(User user)
    {
        return new FriendViewModel()
        {
            ID = user.ID,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            ProfilePhoto = user.ProfilePhoto
        };
    }
}
