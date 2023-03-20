using WeConnect.Data;
namespace WeConnect.Models;
public class PostService 
{
    PostsDB _postsDB;
    CrudDB<Post> _crudDB;
    public PostService(PostsDB postsDB, CrudDB<Post> crudDB)
    {
        _postsDB = postsDB;
        _crudDB = crudDB;
    }
    public async Task<int?> Create(Post post)
    {
        return _crudDB.Create(post, QueryGenerator<Post>.InsertQuery(post));
    }
    public async Task<List<Post>> GetBySearch(string search, User user)
    {
        List<Post> searchedPosts = new();
        try
        {
            List<Post> allPosts = _crudDB.GetAll(user, QueryGenerator<Post>.SelectQuery(new Post(), user));
            searchedPosts = allPosts.Where(p => p.Content == search).ToList();
        }
        catch (InvalidOperationException e)
        {
           return null;
        }
        return searchedPosts;
    }
    public async Task<Post> GetOne(int postId, User user)  //ska lösas via sql
    {
         Post post = _postsDB.GetOne(postId, user);
        return post;
    }
    public async Task<int?> Remove(Post post)   //man ska kunna radera sin post, alltså sätta till ej synlig
    {
        return _crudDB.Delete(post, QueryGenerator<Post>.DeleteQuery(post));
    }
    public async Task<int?> Update(Post post)   //redigera sin post och lägg till is_edited i table
    {
        return _crudDB.Update(post, QueryGenerator<Post>.UpdateQuery(post));
    }
    public async Task<List<Post>> GetAll(int userId, User user)
    {
        try
        {
            List<Post> allPosts = _postsDB.GetById(userId, user);
            return allPosts;
        }
        catch (NullReferenceException)
        {
            return null;
        }
    }
}