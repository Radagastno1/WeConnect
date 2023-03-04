namespace WeConnect.Models;
public class Notification
{
    public int Id{get;set;}
    public string NotificationType{get;set;}
    public string Description{get;set;}
    public string FromUser{get;set;}
    public string ToUser{get;set;}
    public Notification()
    {

    }
    public override string ToString()
    {
        return $"{FromUser} {Description}";
    }
}