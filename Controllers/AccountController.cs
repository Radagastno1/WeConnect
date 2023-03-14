using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeConnect.Models;
using WeConnect.ViewModels;
using Newtonsoft.Json;

namespace WeConnect.Controllers;

public class AccountController : Controller
{
    private readonly FriendService _friendService;
    private readonly UserService _userService;
    private readonly ConversationService _conversationService;
    private readonly NotificationService _notificationService;

    public AccountController(
        FriendService friendService,
        UserService userService,
        ConversationService conversationService,
        NotificationService notificationService
    )
    {
        _friendService = friendService;
        _userService = userService;
        _conversationService = conversationService;
        _notificationService = notificationService;
    }

    public IActionResult Index()
    {
        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = _userService.GetUserById(userId);
            var notifications =
                _notificationService.GetUnreadNotifications(user) ?? new List<Notification>();

            var myViewModel = UserToMyViewModel(user, notifications);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(myViewModel);
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

    // public ActionResult Notifications()
    // {
    //     var userId = HttpContext.Session.GetInt32("UserId");
    //     var user = _userService.GetUserById(userId);
    //     var notifications = _notificationService.GetUnreadNotifications(user);

    //     _notificationService.UpdateToRead(user);
    //     var serializedNotifications = JsonConvert.SerializeObject(notifications);
    //     return View(serializedNotifications);
    // }

    [HttpPost]
    public ActionResult MarkNotificationsAsRead()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        var user = _userService.GetUserById(userId);

        _notificationService.UpdateToRead(user);
        return RedirectToAction("Index", "Account");
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

    private MyViewModel UserToMyViewModel(User user, List<Notification> notifications)
    {
        return new MyViewModel
        {
            Id = user.ID,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Password = user.PassWord,
            BirthDate = user.BirthDate,
            Gender = user.Gender,
            AboutMe = user.AboutMe,
            Notifications = NotificationsToViewModels(notifications, user) ?? new List<NotificationViewModel>()
        };
    }

    private List<NotificationViewModel> NotificationsToViewModels(
        List<Notification> notifications,
        User user
    )
    {
        return notifications
            .Select(
                n =>
                    new NotificationViewModel(_notificationService.UpdateToRead)
                    {
                        Id = n.Id,
                        NotificationType = n.NotificationType,
                        ToUser = n.ToUser,
                        FromUser = n.FromUser,
                        Description = n.Description
                    }
            )
            .ToList();
    }
}
