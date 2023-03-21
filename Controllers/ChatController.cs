using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WeConnect.Models;
using WeConnect.ViewModels;

namespace WeConnect.Controllers;

public class ChatController : Controller
{
    private readonly FriendService _friendService;
    private readonly UserService _userService;
    private readonly ConversationService _conversationService;
    private readonly MessgageService _messageService;

    public ChatController(
        FriendService friendService,
        UserService userService,
        ConversationService conversationService,
        MessgageService messgageService
    )
    {
        _friendService = friendService;
        _userService = userService;
        _conversationService = conversationService;
        _messageService = messgageService;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = await _userService.GetUserById(userId);
            List<int> conversationIds = await _conversationService.GetAllMyConversationsIds(user);
            List<Conversation> myConversations = await _conversationService.GetById(
                conversationIds
            );
            IEnumerable<ConversationViewModel> conversationsViewModels = myConversations.Select(
                c => ConversationToViewModel(c)
            );
            return View(conversationsViewModels);
        }
        catch
        {
            return RedirectToAction("index");
        }
    }

    public async Task<IActionResult> ViewConversation(int id)
    {
        //spara messages till conversations meddelande ??
        try
        {
            int userId = HttpContext.Session.GetInt32("UserId").GetValueOrDefault();
            var user = await _userService.GetUserById(userId);
            var messages = await _messageService.GetAll(id, user);
            List<MessageViewModel> messagesViewModels = new();
            if (messages != null)
            {
                messagesViewModels = messages.Select(m => MessageToViewModel(m)).ToList();
            }
            await _conversationService.UpdateConversationAsRead(id, userId);
            return View(ConversationWithMessagesToViewModel(id, GenerateMyMessagesViewModel(user, messagesViewModels)));
        }
        catch
        {
            return RedirectToAction("index");
        }
    }

    [HttpPost]
    public async Task<IActionResult> SendMessage(string content, int conversationId)
    {
        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = await _userService.GetUserById(userId);

            await _messageService.Create(content, senderId: user.ID, conversationId);
            var messages = await _messageService.GetAll(conversationId, user);
            var messagesViewModels = messages.Select(m => MessageToViewModel(m));
            return RedirectToAction("ViewConversation", "Chat", new { id = conversationId });
        }
        catch
        {
            return RedirectToAction("index");
        }
    }

    public async Task<IActionResult> NewConversation()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        var user = await _userService.GetUserById(userId);
        var friends = await _friendService.GetMine(user);
        return View(FriendsToViewModel(friends));
    }

    [HttpPost]
    public async Task<IActionResult> NewConversation(List<int> userIds)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        var user = await _userService.GetUserById(userId);

        List<User> participants = await _userService.GetUsersById(userIds, user);
        int conversationId = await _conversationService.MakeNew(participants, user);

        return RedirectToAction("ViewConversation", "Chat", new { id = conversationId });
    }

    private ConversationViewModel ConversationToViewModel(Conversation conversation)
    {
        return new ConversationViewModel
        {
            ID = conversation.ID,
            ParticipantsNames = conversation.ParticipantsNames
        };
    }
     private ConversationViewModel ConversationWithMessagesToViewModel(int conversationId, MyMessagesViewModel myMessagesViewModels)
    {
        return new ConversationViewModel
        {
            ID = conversationId,
            Messages =  myMessagesViewModels.MessageViewModels ?? new List<MessageViewModel>(),
            MyViewModel = myMessagesViewModels.MyViewModel
        };
    }

    private MessageViewModel MessageToViewModel(Message message)
    {
        return new MessageViewModel
        {
            ID = message.ID,
            Content = message.Content,
            DateCreated = message.DateCreated,
            Reciever = message.Reciever,
            Sender = message.Sender,
            SenderId = message.SenderId,
            ConversationId = message.ConversationId
        };
    }

    private MyMessagesViewModel GenerateMyMessagesViewModel(
        User user,
        List<MessageViewModel> messageViewModels
    )
    {
        return new MyMessagesViewModel
        {
            MyViewModel = UserToMyViewModel(user),
            MessageViewModels = messageViewModels
        };
    }

    private MyViewModel UserToMyViewModel(User user)
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
            ProfilePhoto = user.ProfilePhoto ?? new Photo()
        };
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

    private List<FriendViewModel> FriendsToViewModel(List<User> users)
    {
        return users.Select(u => UserToFriendViewModel(u)).ToList();
    }
}
