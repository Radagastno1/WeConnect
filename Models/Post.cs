using System.ComponentModel;
namespace WeConnect.Models;
public class Post
{
    public readonly int ID;
    public string Content{get;set;}
    public DateTime DateCreated{get;set;}
    public int UserId{get;set;} // detta istället för user
    // public User User{get;private set;} //behövs ej?
    public string FirstName{get;set;}
    public string LastName{get;set;}
    List<Comment>comments = new();
    public Post(){}
    public Post(string aContent, DateTime aDateCreated, int aUserId)
    {
        Content = aContent;
        DateCreated = aDateCreated;
        UserId = aUserId;
    }
    public override string ToString()
    {
        return $"\t[{ID}] {FirstName} {LastName}\n\t{DateCreated}\n\t{Content}";
    }
}