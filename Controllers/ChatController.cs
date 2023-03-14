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
        ConversationService conversationService, MessgageService messgageService
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
            IEnumerable<ConversationViewModel> conversationsViewModels = myConversations.Select( c => ConversationToViewModel(c));
            return View(conversationsViewModels);
        }
        catch
        {
            return RedirectToAction("index");
        }
    }

    public ActionResult Conversation(int conversationId)
    {
        //spara messages till conversations meddelande ??
        try
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = _userService.GetUserById(userId);
            var messages = _messageService.GetAll(conversationId, user);
             var messagesViewModels = messages.Select(m => MessageToViewModel(m));
            return View(messagesViewModels);
        }
        catch
        {
            return RedirectToAction("index");
        }
    }

    [HttpPost]
    public ActionResult SendMessage(Message message, int conversationId)
    {
        try
        {
            //obs meddelandet måste ha conversationid samt user.id på sig här
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = _userService.GetUserById(userId);
            _messageService.Create(message);
            var messages = _messageService.GetAll(conversationId, user);
            var messagesViewModels = messages.Select(m => MessageToViewModel(m));
            return RedirectToAction("Conversation", "Chat", messagesViewModels);
        }
        catch
        {
            return RedirectToAction("index");
        }
    }
    private ConversationViewModel ConversationToViewModel(Conversation conversation)
    {
        return new ConversationViewModel{
            ID = conversation.ID,
            ParticipantsNames = conversation.ParticipantsNames
        };
    }
    private MessageViewModel MessageToViewModel(Message message)
    {
        return new MessageViewModel{
            ID = message.ID,
            Content = message.Content,
            DateCreated = message.DateCreated,
            Reciever = message.Reciever,
            Sender = message.Sender
        };
    }
}
