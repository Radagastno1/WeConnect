using WeConnect.Models;
using Dapper;
using MySqlConnector;
namespace WeConnect.Data;
public class PostsDB 
{
    // public int? Create(Post obj)
    // {
    //     int id = 0;
    //     string query = "INSERT INTO posts (content, users_id, posts_types_id) " +
    //     "VALUES(@Content, @UserId, 1); SELECT LAST_INSERT_ID()";
    //     using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
    //     id = con.ExecuteScalar<int>(query, param: obj);
    //     return id;
    // }
    // public int? Delete(Post post)  //om man väljer att deleta en post, så kommer den bli visible när du loggar in
    // // därför behövs en tilll bool is_deleted som sätts när man deletar istället! 
    // {
    //     int rowsEffected = 0;
    //     string query = "Update posts SET is_deleted = TRUE WHERE id = @Id;";
    //     using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
    //     rowsEffected = con.ExecuteScalar<int>(query, new { @Id = post.ID });
    //     return rowsEffected;
    // }
    // //hämtar alla inlägg som dina vänner har gjort och som är synliga 
    // public List<Post> GetAll(User user)
    // {
    //     List<Post> posts = new();
    //     string query = "SELECT p.id as 'Id', p.content as 'Content', p.date_created as 'DateCreated', " +
    //     "u.first_name as 'FirstName', u.last_name as 'LastName', p.users_id as 'UserId' " +
    //     "FROM posts p INNER JOIN users u " +
    //     "ON p.users_id = u.id " +
    //     "INNER JOIN users_friends uf1 " +
    //     "ON u.id = uf1.users_id1 " +
    //     "WHERE p.posts_types_id = 1 " +
    //     "AND p.is_visible = TRUE " +
    //     "AND p.is_deleted = FALSE " +
    //     "AND uf1.users_id2 = @Id;";
    //     using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
    //     posts = con.Query<Post>(query, param : user).ToList();
    //     if (posts.Count > 0)
    //     {
    //         return posts;
    //     }
    //     else
    //     {
    //         return null;
    //     }
    // }
    // public int? Update(Post obj)
    // {
    //     int rowsEffected = 0;
    //     string query = "UPDATE posts SET content = @Content, is_edited = True WHERE id = @Id;";
    //     using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");

    //     rowsEffected = con.ExecuteScalar<int>(query, new { @Content = obj.Content, @Id = obj.ID });
    //     return rowsEffected;
    // }
    public List<Post> GetById(int userId, User user)
    {
        List<Post> posts = new();
        string query = $"SELECT p.id as 'Id', p.content as 'Content', p.date_created as 'DateCreated', u.first_name as 'FirstName', u.last_name as 'LastName', p.users_id as 'UserId' " +
         $"FROM posts p INNER JOIN users u ON p.users_id = u.id WHERE p.post_type = 'Post' AND p.is_visible = TRUE AND p.is_deleted = FALSE AND p.users_id = @userId " +
         "AND p.users_id not in " +
        "(select blocked_user_id from users_blocked where users_id = @myId) " +
        "AND p.users_id not in " +
        "(select users_id from users_blocked where blocked_user_id = @myId);";
        using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
        posts = con.Query<Post>(query, new { @userId = userId, @myId = user.ID }).ToList();
        if (posts.Count > 0)
        {
            return posts;
        }
        else
        {
            return null;
        }
    }
    public Post GetOne(int postId, User user)  //byt namn på metoden 
    {
        Post post = new();
        string query = $"SELECT p.id as 'Id', p.content as 'Content', p.date_created as 'DateCreated', u.first_name as 'FirstName', u.last_name as 'LastName', p.users_id as 'UserId' " +
         $"FROM posts p INNER JOIN users u ON p.users_id = u.id WHERE p.post_type = 'Post' AND p.is_visible = TRUE AND p.is_deleted = FALSE AND p.id = @postId;";
        using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
        post = con.QuerySingle<Post>(query, new { @postId = postId });
        if (post != null)
        {
            return post;
        }
        else
        {
            return null;
        }
    }
}