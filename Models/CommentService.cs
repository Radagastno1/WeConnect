using WeConnect.Data;
namespace WeConnect.Models;
public class CommentsManager
{
    CommentsDB _commentsDB;
    public CommentsManager(CommentsDB commentsDB)
    {
        _commentsDB = commentsDB;
    }
    public int? Create(Comment obj)
    {
        return _commentsDB.Create(obj, QueryGenerator<Comment>.InsertQuery(obj));
    }
    public List<Comment> GetAll(int postId, User user)
    {
        try
        {
            List<Comment> allComments = _commentsDB.GetById(postId, user);
            return allComments;
        }
        catch(InvalidOperationException)
        {
            return null;
        }
    }
    public List<Comment> GetBySearch(string name, User user)
    {
        throw new NotImplementedException();  //sök efter kommentarer i en post via namn?
    }
    public Comment GetOne(int data, User user)
    {
        throw new NotImplementedException();  //hämta specifik kommentar
    }
    public int? Remove(Comment obj)
    {
        throw new NotImplementedException();  //måste kunna radera sin egen kommentar ELLER om den är på sitt inlägg
    }
    public int? Update(Comment comment)
    {
        throw new NotImplementedException();  //redigera sin egna kommentar
    }
}