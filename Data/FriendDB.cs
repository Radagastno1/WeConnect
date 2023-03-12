using WeConnect.Models;
using Dapper;
using MySqlConnector;

namespace WeConnect.Data;

public class FriendsDB
{
    public int Create(User user, int friendId)
    {
        using MySqlConnection connection = new MySqlConnection(
            $"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;"
        );
        string query =
            "INSERT INTO users_friends (users_id1, users_id2) VALUES(@userId, @friendId);";
        return connection.ExecuteScalar<int>(
            query,
            new { @userId = user.ID, @friendId = friendId }
        );
    }

    public int Delete(User user, int friendId)
    {
        using MySqlConnection connection = new MySqlConnection(
            $"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;"
        );
        string query = "CALL DeleteFriendship(@userId, @friendId);";
        // "START TRANSACTION;" +
        // "DELETE FROM users_friends WHERE users_id1 = @userId AND users_id2 = @friendId;" +
        // "DELETE FROM users_friends WHERE users_id1 = @friendId AND users_id2 = @userId;" +
        // "COMMIT;";
        int rows = connection.ExecuteScalar<int>(
            query,
            new { @userId = user.ID, @friendId = friendId }
        );
        return rows;
    }

    public List<User> GetMine(User user)
    {
        List<User> friends = new();
        using MySqlConnection connection = new MySqlConnection(
            $"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;"
        );
        string query =
            "SELECT u.id AS 'Id', u.first_name AS 'FirstName', u.last_name AS 'LastName', DATE_FORMAT(u.birth_date, '%Y-%m-%d') as 'BirthDate', u.gender, "
            + "u.about_me as 'AboutMe' FROM users u "
            + "INNER JOIN users_friends uf ON u.id = uf.users_id2 "
            + "WHERE uf.users_id1 = @userId AND uf.is_accepted = TRUE;";
        friends = connection.Query<User>(query, new { @userId = user.ID }).ToList();
        return friends;
    }

    public List<int> GetMyFriendRequests(User user)
    {
        List<int> ids = new();
        using MySqlConnection connection = new MySqlConnection(
            $"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;"
        );
        string query = "SELECT users_id2 " + "FROM users_friends  " + "WHERE users_id1 = @userId;";
        ids = connection.Query<int>(query, new { @userId = user.ID }).ToList();
        return ids;
    }

    public int CheckIfFriendAccepted(User user, int friendId)
    {
        try
        {
            using MySqlConnection connection = new MySqlConnection(
                $"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;"
            );
            string query =
                "SELECT id FROM users_friends WHERE users_id1 = @friendId AND users_id2 = @userId";
            int id = connection.QuerySingle<int>(
                query,
                new { @userId = user.ID, @friendId = friendId }
            );
            return id;
        }
        catch (InvalidOperationException)
        {
            return 0;
        }
    }

    public int CheckIfBefriended(User user, int friendId)
    {
        using MySqlConnection connection = new MySqlConnection(
            $"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;"
        );
        string query =
            "SELECT id FROM users_friends WHERE users_id1 = @userId AND users_id2 = @friendId";
        int id = connection.QuerySingle<int>(
            query,
            new { @userId = user.ID, @friendId = friendId }
        );
        return id;
    }

    public int CheckIfFriend(User user, int friendId)
    {
        using MySqlConnection connection = new MySqlConnection(
            $"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;"
        );
        string query =
            "SELECT id FROM users_friends WHERE users_id1 = @userId AND users_id2 = @friendId " + 
            "AND is_accepted = true";
        int id = connection.QuerySingle<int>(
            query,
            new { @userId = user.ID, @friendId = friendId }
        );
        return id;
    }

    public int Update(User user, int friendId) //uppdaterar till att man har accepterat förfrågan
    {
        using MySqlConnection connection = new MySqlConnection(
            $"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;"
        );
        string query =
            "START TRANSACTION;"
            + "UPDATE users_friends SET is_accepted = TRUE WHERE users_id1 = @userId AND users_id2 = @friendId;"
            + "UPDATE users_friends SET is_accepted = TRUE WHERE users_id1 = @friendId AND users_id2 = @userId;"
            + "COMMIT;";
        int row = connection.ExecuteScalar<int>(
            query,
            new { @userId = user.ID, @friendId = friendId }
        );
        return row;
    }
}
