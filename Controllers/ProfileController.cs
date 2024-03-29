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

    public async Task<IActionResult> ShowProfile(int id)
    {
        try
        {
            //this is me
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = await _userService.GetUserById(userId);
            //kollar om den finns bland mina vänner, ev ladda hem mina vänner från början??
            var friendsAsUsers = await _friendService.GetMine(user);
            var foundFriend = friendsAsUsers.Find(f => f.ID == id);
            if (foundFriend == null)
            {
                //OBS måste komma logik som rensar bort blockerade eller som blockerat mig!!
                var userToVisit = await _userService.GetUserById(id);

                return RedirectToAction("NonFriend", "Profile", UserToFriendViewModel(userToVisit));
            }
            // var friendToVisit = await _userService.GetUserById(foundFriend);
            return RedirectToAction("Friend", "Profile", UserToFriendViewModel(foundFriend));
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

    public IActionResult Error()
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
            ProfilePhoto = user.ProfilePhoto ?? new Photo(),
            FriendStatus = SetFriendStatus(user)
        };
    }

    private string SetFriendStatus(User friend)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        var user = _userService.GetUserById(userId).Result;
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
