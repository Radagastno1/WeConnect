using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeConnect.ViewModels;
using WeConnect.Models;

namespace WeConnect.Controllers;

public class ProfileController : Controller
{
    private readonly FriendService _friendService;
    private readonly UserService _userService;
    private readonly ConversationService _conversationService;

    public ProfileController(
        FriendService friendService,
        UserService userService,
        ConversationService conversationService
    )
    {
        _friendService = friendService;
        _userService = userService;
        _conversationService = conversationService;
    }

    // public IActionResult Index()
    // {
    //     try
    //     {
    //         var userId = HttpContext.Session.GetInt32("UserId");
    //         var user = _userService.GetUserById(userId);
    //         if (user == null)
    //         {
    //             return RedirectToAction("Index", "Account");
    //         }
    //         var homePageViewModel = new HomePageViewModel
    //         {
    //             Id = user.ID,
    //             FirstName = user.FirstName,
    //             LastName = user.LastName,
    //             Email = user.Email
    //         };
    //         return View(homePageViewModel);
    //     }
    //     catch (Exception e)
    //     {
    //         Debug.WriteLine("ERROR:" + e.Message);
    //         return RedirectToAction("Index", "Home");
    //     }
    // }

    public ActionResult<FriendViewModel> Friend(int? id)
    {
        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = _userService.GetUserById(userId);
            var friendsAsUsers = _friendService.GetMine(user);
            var friendsAsViewModels = FriendsToViewModel(friendsAsUsers);
            var friendToVisit = friendsAsViewModels.Find(f => f.ID == id);
            return View(friendToVisit);
        }
        catch
        {
            return RedirectToAction("Index", "Account");
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
            BirthDate = user.BirthDate,
            Gender = user.Gender,
            AboutMe = user.AboutMe
        };
    }
}
