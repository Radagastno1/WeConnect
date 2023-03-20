using WeConnect.Data;
namespace WeConnect.Models;
public class CommentsManager
{
    CommentsDB _commentsDB;
     CrudDB<Comment> _crudDB;
    public CommentsManager(CommentsDB commentsDB, CrudDB<Comment> crudDB)
    {
        _commentsDB = commentsDB;
        _crudDB = crudDB;
    }
    public async Task<int?> Create(Comment obj)
    {
        return _crudDB.Create(obj, QueryGenerator<Comment>.InsertQuery(obj));
    }
    public async Task<List<Comment>> GetAll(int postId, User user)
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
    public async Task<List<Comment>> GetBySearch(string name, User user)
    {
        throw new NotImplementedException();  //sök efter kommentarer i en post via namn?
    }
    public async Task<Comment> GetOne(int data, User user)
    {
        throw new NotImplementedException();  //hämta specifik kommentar
    }
    public async Task<int?> Remove(Comment obj)
    {
        throw new NotImplementedException();  //måste kunna radera sin egen kommentar ELLER om den är på sitt inlägg
    }
    public async Task<int?> Update(Comment comment)
    {
        throw new NotImplementedException();  //redigera sin egna kommentar
    }
}