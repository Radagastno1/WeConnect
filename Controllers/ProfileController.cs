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
    public ActionResult ShowProfile(int id)
    {
        try
        {
            //this is me
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = _userService.GetUserById(userId);
            //kollar om den finns bland mina vänner, ev ladda hem mina vänner från början??
            var friendsAsUsers = _friendService.GetMine(user);
            var friendToVisit = friendsAsUsers.Find(f => f.ID == id);
            if (friendToVisit == null)
            {
                //OBS måste komma logik som rensar bort blockerade eller som blockerat mig!!
                var userToVisit = _userService.GetUserById(id);

                return RedirectToAction("NonFriend", "Profile", UserToFriendViewModel(userToVisit));
            }
            return RedirectToAction("Friend", "Profile", UserToFriendViewModel(friendToVisit));
        }
        catch
        {
            return RedirectToAction("Error", "Profile");
        }
    }

    public ActionResult<FriendViewModel> Friend(FriendViewModel friend)
    {
        try
        {
            return View(friend);
        }
        catch
        {
            return RedirectToAction("Error", "Profile");
        }
    }

    public ActionResult<FriendViewModel> NonFriend(FriendViewModel userToVisit)
    {
        try
        {
            //BEHÖVER GÖRA EN NONFRIENDVIEWMODEL och kolla lägga till publikt osv.....
            return View(userToVisit);
        }
        catch
        {
            return RedirectToAction("Error", "Profile");
        }
    }

    public ActionResult Error()
    {
        return View();
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
            AboutMe = user.AboutMe,
            ProfilePhoto = user.ProfilePhoto,
            FriendStatus = SetFriendStatus(user)
        };
    }

    private string SetFriendStatus(User friend)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        var user = _userService.GetUserById(userId);
        string status = string.Empty;
        if (_friendService.IsFriends(user, friend.ID))
        {
            status = "Friends";
        }
        else if (_friendService.IsBefriended(user, friend.ID))
        {
            status = "Friend request waiting";
        }
        else if (_friendService.IsFriendRequestWaiting(user, friend.ID))
        {
            status = "Confirm friend request";
        }
        else
        {
            status = "Add friend";
        }
        return status;
    }
}
