using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeConnect.Models;
using WeConnect.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WeConnect.Controllers;

public class AccountController : BaseController
{
    private readonly FriendService _friendService;
    private readonly UserService _userService;
    private readonly ConversationService _conversationService;
    private readonly NotificationService _notificationService;
    private readonly IConfiguration _configuration;

    public AccountController(
        FriendService friendService,
        UserService userService,
        ConversationService conversationService,
        NotificationService notificationService,
        IConfiguration configuration
    )
        : base(configuration)
    {
        _friendService = friendService;
        _userService = userService;
        _conversationService = conversationService;
        _notificationService = notificationService;
        _configuration = configuration;
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    // [Authorize]
    public async Task<IActionResult> Index()
    {
        Console.WriteLine("AccountControllers Index action is called.");
        try
        {
            // var jwtTokenString = Request.Cookies["AuthCookie"];
            var jwtTokenString = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(jwtTokenString);

            // Hämta användarens id från JWT-tokenet
            var userId = int.Parse(
                token.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value
            );

            // var userId = GetUserIdFromToken(jwtTokenString);
            var user = await _userService.GetUserById(userId);

            var notifications =
                await _notificationService.GetUnreadNotifications(user) ?? new List<Notification>();

            var conversations = await _conversationService.GetUnreadConversations(user);

            var myViewModel = UserToMyViewModel(user, notifications, conversations);
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

    public async Task<ActionResult<FriendViewModel>> MyFriends()
    {
        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = await _userService.GetUserById(userId);

            var friendsAsUsers = await _friendService.GetMine(user);
            var friendsAsViewModels = FriendsToViewModel(friendsAsUsers);
            return View(friendsAsViewModels);
        }
        catch
        {
            return RedirectToAction("index");
        }
    }

    public async Task<ActionResult> SignOut()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Search()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult<FriendViewModel>> Search(string search)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        var user = await _userService.GetUserById(userId);
        var foundUsers = await _userService.GetBySearch(search, user);
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
    public async Task<ActionResult> MarkNotificationsAsRead()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        var user = _userService.GetUserById(userId).Result;

        await _notificationService.UpdateToRead(user);
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
            ProfilePhoto = user.ProfilePhoto ?? new Photo()
        };
    }

    private MyViewModel UserToMyViewModel(
        User user,
        List<Notification> notifications,
        List<Conversation> conversations
    )
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
            Notifications =
                NotificationsToViewModels(notifications, user) ?? new List<NotificationViewModel>(),
            Conversations =
                ConversationsToViewModels(conversations) ?? new List<ConversationViewModel>(),
            ProfilePhoto = user.ProfilePhoto ?? new Photo()
        };
    }

    private List<ConversationViewModel> ConversationsToViewModels(List<Conversation> conversations)
    {
        List<ConversationViewModel> conversationViewModels = conversations
            .Select(
                c =>
                    new ConversationViewModel { ID = c.ID, ParticipantsNames = c.ParticipantsNames }
            )
            .ToList();
        return conversationViewModels;
    }

    private List<NotificationViewModel> NotificationsToViewModels(
        List<Notification> notifications,
        User user
    )
    {
        return notifications
            .Select(
                n =>
                    new NotificationViewModel()
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
