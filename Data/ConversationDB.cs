using WeConnect.Models;
using Dapper;
using MySqlConnector;
namespace WeConnect.Data;
public class ConversationDB
{
    // public int? Create(Conversation conversation)
    // {
    //     string query = "INSERT INTO conversations(creator_id) VALUES(@CreatorId);" +
    //     "SELECT LAST_INSERT_ID();";
    //     using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
    //     int conversationId = con.ExecuteScalar<int>(query, param: conversation);
    //     return conversationId;
    // }
    // public int? Update(Conversation conversation)
    // {
    //     string query = "INSERT INTO users_conversations(users_id, conversations_id) VALUES (@participantId, @Id);" +
    //     "SELECT LAST_INSERT_ID();";
    //     using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
    //     int usersConversationId = con.ExecuteScalar<int>(query, param: conversation);
    //     return usersConversationId;
    // }
    // public int? Delete(Conversation obj)
    // {
    //     throw new NotImplementedException();
    // }
    // public List<Conversation> GetAll(User user)
    // {
    //     List<Conversation> allConversations = new();
    //     string query = "SELECT uc.conversations_id as 'Id', c.date_created as 'DateCreated', c.creator_id as 'CreatorId', u.id as 'ParticipantId'" +
    //                    "FROM conversations c " +
    //                    "INNER JOIN users_conversations uc " +
    //                    "ON c.id = uc.conversations_id " +
    //                    "INNER JOIN users u " +
    //                     "ON u.id = uc.users_id " +
    //                     "WHERE u.is_active = true " +
    //                     "AND u.id = @ID;";
    //     using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
    //     allConversations = con.Query<Conversation>(query, param: user).ToList();
    //     return allConversations;
    // }
    //denna ska användas för att hämta konv mellan specifika användare, inte implementerat det i c# nu
    public List<Conversation> GetConversationsOfSpecificParticipants(int amountOfUsers, string sql)
    {   
        List<Conversation> conversations = new();
        string query = $"SELECT uc.conversations_id AS 'ID', " +
        "GROUP_CONCAT(uc.users_id) AS User_List " +
        "FROM users_conversations uc " +
        $"WHERE  uc.users_id IN ({sql})" +
        "GROUP BY uc.conversations_id " +
        "HAVING COUNT(uc.users_id) = @amountOfUsers;";
        using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
        conversations = con.Query<Conversation>(query, new { @amountOfUsers = amountOfUsers }).ToList();
        return conversations;
    }
    public ConversationResult GetConversationIdAndParticipantNames(int conversationId)
    {
        List<User> users = new();
        ConversationResult result = new();
        string query = $" SELECT c.id as 'ID', GROUP_CONCAT(u.first_name) AS ParticipantsNames " +
                       "FROM conversations c " +
                       "INNER JOIN users_conversations uc " +
                       "ON uc.conversations_id = c.id " +
                       "INNER JOIN users u " +
                       "ON u.id = uc.users_id " +
                       "WHERE c.id = @id AND u.is_active = true;";
        using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
        result.Conversation = con.QuerySingle<Conversation>(query, new { @id = conversationId });
        if (result.Conversation != null)
        {
            result.ConversationExists = true;
        }
        return result;
    }
    public List<Conversation> GetById(int id, User user)
    {
        // List<Conversation> conversations = new();
        // string query = $"SELECT uc.conversation_id as 'ID' FROM users_conversations uc WHERE uc.users_id = @id;";
        // using (MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;"))
        // {
        //     conversations = con.Query<Conversation>(query).ToList();
        // }
        // return conversations;
        throw new NotImplementedException();
    }
    public Conversation GetDialogueId(int userId, int id)
    {
        Conversation dialogue = new();
        string query = "select uc.conversations_id as 'Id'" +
                        "from users_conversations uc " +
                        "group by uc.conversations_id " +
                        "having sum(uc.users_id in (@userId, @id)) = count(*);";
        using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
        dialogue = con.QuerySingle<Conversation>(query, new { @userId = userId, @id = id });
        return dialogue;
    }
}