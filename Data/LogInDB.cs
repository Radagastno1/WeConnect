using WeConnect.Models;
using Dapper;
using MySqlConnector;
namespace WeConnect.Data;
public class LogInDB 
{
    public User GetMemberByLogIn(string email, string passWord)
    {
        User user = new();
        string query =
        "SELECT u.id as 'Id', u.first_name as 'FirstName', u.last_name as 'LastName', " +
        "u.email as 'Email', u.pass_word as 'PassWord', u.birth_date as 'BirthDate', u.gender as 'Gender', u.about_me as 'AboutMe' " +
        "FROM users u " +
        "WHERE email = @Email AND pass_word = @PassWord AND u.role_id = 5 AND is_deleted = FALSE;";
        using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;Allow User Variables=True;");
        user = con.QuerySingle<User>(query, new { @Email = email, @PassWord = passWord });
        return user;
    }
    public int UpdateToActivated(int userId)
    {
        string query = "START TRANSACTION;" +
        "UPDATE users SET is_active = TRUE WHERE id = @id;" +
        "UPDATE messages SET is_visible = TRUE WHERE sender_id = @id; " +
        "UPDATE posts SET is_visible = TRUE WHERE users_id = @id; " +
        "COMMIT;";
        using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;Allow User Variables=True;");
        int rows = con.ExecuteScalar<int>(query, new { @id = userId });
        return rows;   //returnerar ej rows
    }
}