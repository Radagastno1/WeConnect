using WeConnect.Models;
using Dapper;
using MySqlConnector;
namespace WeConnect.Data;
public class UsersDB 
{
     public User GetUserById(int? id)
    {
        User foundUser = new();
        try
        {
            string query = 
            "START TRANSACTION;" +
                "SELECT u.id, u.first_name as 'FirstName', u.last_name as 'LastName', " +
               "DATE_FORMAT(u.birth_date, '%Y-%m-%d') as 'BirthDate', u.gender, " +
               "u.about_me as 'AboutMe' " +
               "FROM users u " +
               "WHERE u.role_id = 5 AND u.is_deleted = false AND id = @id;" +
                "SELECT u.profile_photo FROM users u " +
                "INNER JOIN photo_album pa " + 
                "ON pa.album_id = u.id " +
                "INNER JOIN photos p " + 
                "ON p.photo_album_id = pa.id " + 
                "WHERE u.id = @id;" +
                "COMMIT;";
            using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
            foundUser = con.QuerySingle<User>(query, new { @id = id});
            return foundUser;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }
    // public int? Create(User obj)
    // {
    //     string query =
    //     "INSERT INTO users(first_name, last_name, email, pass_word, birth_date, gender, about_me, role_id) " +
    //     "VALUES(@FirstName, @LastName, @Email, @PassWord, @BirthDate, @Gender, @AboutMe, 5); " +
    //     "SELECT LAST_INSERT_ID();";
    //     try
    //     {
    //         using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
    //         int userId = con.ExecuteScalar<int>(query, param: obj);
    //         return userId;
    //     }
    //     catch (InvalidOperationException)
    //     {
    //         return null;
    //     }
    // }
    // public int? Delete(User user)
    // {
    //     int rowsEffected = 0;
    //     string query = "START TRANSACTION;" +
    //     "UPDATE users SET is_active = false WHERE id = @id;" +
    //     "UPDATE messages SET is_visible = FALSE WHERE sender_id = @id;" +
    //     "UPDATE posts SET is_visible = FALSE WHERE users_id = @id;" +
    //     "COMMIT;";
    //     using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
    //     rowsEffected = con.ExecuteScalar<int>(query, param: user);
    //     return rowsEffected;
    // }
    // public List<User> GetAll(User user)
    // {
    //     List<User> users = new();
    //     string query =
    //     "SELECT u.id, u.first_name as 'FirstName', u.last_name as 'LastName', u.email," +
    //     "u.pass_word as 'PassWord', DATE_FORMAT(u.birth_date, '%Y-%m-%d') as 'BirthDate', u.gender, u.about_me as 'AboutMe', r.name as 'Role' " +
    //     "FROM users u " +
    //     "INNER JOIN roles r ON r.id = ur.roles_id WHERE u.is_deleted = false;";
    //     using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
    //     users = con.Query<User>(query).ToList();
    //     return users;
    // }
    // public int? Update(User user)
    // {
    //     int rowsEffected = 0;
    //     string query = "Update users SET first_name = @FirstName, last_name = @LastName, " +
    //     "email = @Email, pass_word = @PassWord, gender = @Gender, " +
    //     "about_me = @AboutMe WHERE id = @ID;";
    //     using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
    //     rowsEffected = con.ExecuteScalar<int>(query, param: user);
    //     return rowsEffected;
    // }
    public List<User> GetSearches(string name)
    {
        List<User> foundUsers = new();
        string query = "SELECT u.id as 'ID' FROM users u " +
        $"WHERE u.first_name LIKE '%{name}%' OR u.last_name LIKE '%{name}%' AND u.is_active = true;";
        //SÖKNING FÅR SKE ATT FÖRST KÖRS DENNA - OCH SEDAN HÄMTAS VAR OCH EN AV DE HITTADE USERS GENOM GETBYID
        using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
        foundUsers = con.Query<User>(query, new { @name = name }).ToList();
        return foundUsers;
    }
    public User GetOne(int id, User user)
    {
        User foundUser = new();
        try
        {
            string query = "SELECT u.id, u.first_name as 'FirstName', u.last_name as 'LastName', " +
               "DATE_FORMAT(u.birth_date, '%Y-%m-%d') as 'BirthDate', u.gender, " +
               "u.about_me as 'AboutMe' " +
               "FROM users u " +
               "WHERE u.role_id = 5 AND u.is_deleted = false " +
               "AND u.id = @id " +
               "AND u.id not in (select blocked_user_id from users_blocked where users_id = @userId) " +
               "AND u.id not in (select users_id FROM users_blocked where blocked_user_id = @userId);";
            using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
            foundUser = con.QuerySingle<User>(query, new { @id = id, @userId = user.ID });
            return foundUser;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }
    public List<User> GetInactive()
    {
        List<User> users = new();
        string query = "SELECT u.id as 'Id', DATE_ADD(u.date_inactive, interval 30 day) " +
        "as deletingdate FROM users u  WHERE DATE_ADD(u.date_inactive, interval 30 day) < CURRENT_DATE() " +
        "AND is_deleted = false;";
        try
        {
            using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;Allow User Variables=true;");
            users = con.Query<User>(query).ToList();
            return users;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }
    public void UpdateToDeleted(User user)
    {
        string query = "START TRANSACTION; " +
        "UPDATE users SET is_deleted = true WHERE id = @Id; " +
        "UPDATE messages SET is_visible = false WHERE sender_id = @Id;" +
        "COMMIT;";
        using (MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;Allow User Variables=true;"))
            con.ExecuteScalar<int>(query, param: user);
    }
}