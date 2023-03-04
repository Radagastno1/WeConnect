namespace WeConnect.Models;
public class Comment : Post
{
    public int OnPostId{get; private set;}

    public Comment(){}
    public Comment(string aContent, DateTime aDateCreated, int aUserId, int aOnPostId) : base(aContent, aDateCreated, aUserId)
    {
        OnPostId = aOnPostId;
    }

    public override string ToString()
    {
        return $"\t{FirstName} {LastName}\n\t{Content}";
    }

}