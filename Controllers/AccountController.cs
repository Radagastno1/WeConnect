using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeConnect.Models;
using WeConnect.ViewModels;

namespace WeConnect.Controllers;

public class AccountController : Controller
{
    private readonly FriendService _friendService;
    private readonly UserService _userService;
    private readonly ConversationService _conversationService;

    public AccountController(
        FriendService friendService,
        UserService userService,
        ConversationService conversationService
    )
    {
        _friendService = friendService;
        _userService = userService;
        _conversationService = conversationService;
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

    public ActionResult<Conversation> Chat()
    {
        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = _userService.GetUserById(userId);
            List<int> conversationIds = _conversationService.GetAllMyConversationsIds(user);
            List<Conversation> myConversations = _conversationService.GetById(conversationIds);
            return View(myConversations);
        }
        catch
        {
            return RedirectToAction("index");
        }
    }

    public ActionResult SignOut()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    public ActionResult Search()
    {
        return View();
    }

    [HttpPost]
    public ActionResult<FriendViewModel> Search(string search)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        var user = _userService.GetUserById(userId);
        var foundUsers = _userService.GetBySearch(search, user);
        var usersAsFriendViewModels = foundUsers.Select(u => UserToFriendViewModel(u)).ToList();
        return View(usersAsFriendViewModels);
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
