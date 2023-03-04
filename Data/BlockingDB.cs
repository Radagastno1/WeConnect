using WeConnect.Models;
using Dapper;
using MySqlConnector;

namespace WeConnect.Data;

public class BlockingsDB
{
    public int Create(User user, int blockedUserId)
    {
        int blockedId = 0;
        string query =
            "INSERT INTO users_blocked (users_id, blocked_user_id) VALUES(@userId, @blockedUserId);"
            + "SELECT LAST_INSERT_ID();";
        using MySqlConnection con = new MySqlConnection(
            $"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;"
        );
        blockedId = con.ExecuteScalar<int>(
            query,
            new { @userId = user.ID, @blockedUserId = blockedUserId }
        );
        return blockedId;
    }

    public int Delete(User user, int blockedUserId)
    {
        string query =
            "DELETE FROM users_blocked WHERE users_id = @userId AND blocked_user_id = @blockedId;";
        using MySqlConnection con = new MySqlConnection(
            $"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;"
        );
        int rows = con.ExecuteScalar<int>(
            query,
            new { @userId = user.ID, @blockedUserId = blockedUserId }
        );
        return rows;
    }

    public List<User> GetMine(User user)
    {
        string query =
            "SELECT u.id, u.first_name AS 'FirstName', u.last_name AS 'LastName' FROM users u "
            + "INNER JOIN users_blocked ub ON u.id = ub.blocked_user_id "
            + "WHERE ub.users_id = @userId;";
        using MySqlConnection con = new MySqlConnection(
            $"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;"
        );
        List<User> blockedUsers = con.Query<User>(query, new { @userId = user.ID }).ToList();
        return blockedUsers;
    }

    public int Update(User obj, int blockedUserId)
    {
        throw new NotImplementedException();
    }
}
