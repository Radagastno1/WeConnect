using Microsoft.AspNetCore.Mvc.Rendering;
using WeConnect.Models;

namespace WeConnect.ViewModels;

[Serializable]
public class MyViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string BirthDate { get; set; }
    public string Gender { get;  set; }
    public string AboutMe { get; set; }
    public List<FriendViewModel> MyFriends { get; set; } = new();
    public List<NotificationViewModel> Notifications{get;set;} = new();
    public List<ConversationViewModel> Conversations{get;set;} = new();
}
