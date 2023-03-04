using WeConnect.Data;
namespace WeConnect.Models;
public class PostService 
{
    PostsDB _postsDB;
    public PostService(PostsDB postsDB)
    {
        _postsDB = postsDB;
    }
    public int? Create(Post post)
    {
        return _postsDB.Create(post, QueryGenerator<Post>.InsertQuery(post));
    }
    public List<Post> GetBySearch(string search, User user)
    {
        List<Post> searchedPosts = new();
        try
        {
            List<Post> allPosts = _postsDB.GetAll(user, QueryGenerator<Post>.SelectQuery(new Post(), user));
            searchedPosts = allPosts.Where(p => p.Content == search).ToList();
        }
        catch (InvalidOperationException e)
        {
           return null;
        }
        return searchedPosts;
    }
    public Post GetOne(int postId, User user)  //ska lösas via sql
    {
         Post post = _postsDB.GetOne(postId, user);
        return post;
    }
    public int? Remove(Post post)   //man ska kunna radera sin post, alltså sätta till ej synlig
    {
        return _postsDB.Delete(post, QueryGenerator<Post>.DeleteQuery(post));
    }
    public int? Update(Post post)   //redigera sin post och lägg till is_edited i table
    {
        return _postsDB.Update(post, QueryGenerator<Post>.UpdateQuery(post));
    }
    public List<Post> GetAll(int userId, User user)
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