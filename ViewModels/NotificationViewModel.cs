using WeConnect.Models;
namespace WeConnect.ViewModels;

public class NotificationViewModel
{
    public int Id { get; set; }
    public string NotificationType { get; set; }
    public string Description { get; set; }
    public string FromUser { get; set; }
    public string ToUser { get; set; }
    public Action<User> _notificationRead;

    public NotificationViewModel(Action<User> notificationRead)
    {
        _notificationRead = notificationRead;
    }
     public NotificationViewModel()
    {
    
    }
    public void OnNotificationRead(User user)
    {
        _notificationRead?.Invoke(user);
    }
}
