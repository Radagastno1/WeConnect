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

    public ActionResult Index()
    {
        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = _userService.GetUserById(userId);
            List<int> conversationIds = _conversationService.GetAllMyConversationsIds(user);
            List<Conversation> myConversations = _conversationService.GetById(conversationIds);
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

    public ActionResult ViewConversation(int id)
    {
        //spara messages till conversations meddelande ??
        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = _userService.GetUserById(userId);
            var messages = _messageService.GetAll(id, user);
            List<MessageViewModel> messagesViewModels = messages
                .Select(m => MessageToViewModel(m))
                .ToList();
            return View(GenerateMyMessagesViewModel(user, messagesViewModels));
        }
        catch
        {
            return RedirectToAction("index");
        }
    }

    [HttpPost]
    public ActionResult SendMessage(string content, int conversationId)
    {
        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = _userService.GetUserById(userId);

            _messageService.Create(content, senderId: user.ID, conversationId);
            var messages = _messageService.GetAll(conversationId, user);
            var messagesViewModels = messages.Select(m => MessageToViewModel(m));
            return RedirectToAction("ViewConversation", "Chat", new{id = conversationId});
        }
        catch
        {
            return RedirectToAction("index");
        }
    }

    private ConversationViewModel ConversationToViewModel(Conversation conversation)
    {
        return new ConversationViewModel
        {
            ID = conversation.ID,
            ParticipantsNames = conversation.ParticipantsNames
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
            AboutMe = user.AboutMe
        };
    }
}
