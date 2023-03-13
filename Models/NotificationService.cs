using WeConnect.Data;
namespace WeConnect.Models;
public class NotificationService
{
    //utv: man ska kunna välja notis och komma till själva händelsen, tex vänförfråg-sida
    NotificationsDB _notificationsDB;
    public NotificationService(NotificationsDB notificationsDB)
    {
        _notificationsDB = notificationsDB;
    }
    public List<Notification> GetUnreadNotifications(User user)
    {
        try
        {
            List<Notification> notifications =  _notificationsDB.GetUnread(user);
            if(notifications.Count < 1 || notifications == null)
            {
                throw new InvalidOperationException();
            }
            else
            {
                return notifications;
            }
        }
        catch(InvalidOperationException)
        {
            return null;
        }
    }
    public void UpdateToRead(User user)
    {
        _notificationsDB.UpdateToRead(user);
    }
}