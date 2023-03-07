using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeConnect.Models;

namespace WeConnect.Controllers;

public class AccountController : Controller
{
    private readonly FriendService _friendService;
    private readonly UserService _userService;

    public AccountController(FriendService friendService, UserService userService)
    {
        _friendService = friendService;
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

    public ActionResult<FriendViewModel> MyFriends()
    {
        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = _userService.GetUserById(userId);
            var friendsAsUsers = _friendService.GetMine(user);
            var friendsAsViewModels = FriendsToViewModel(friendsAsUsers);
            return View(friendsAsViewModels);
        }
        catch
        {
            return RedirectToAction("index");
        }
    }

    private List<FriendViewModel> FriendsToViewModel(List<User> users)
    {
        return users.Select(u => UserToFriendViewModel(u)).ToList();
    }
    private FriendViewModel UserToFriendViewModel(User user)
    {
        return new FriendViewModel(){
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };
    }

}
