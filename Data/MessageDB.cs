using WeConnect.Models;
using Dapper;
using MySqlConnector;
namespace WeConnect.Data;
public class MessagesDB
{
    // public int? Create(Message obj, string testar) 
    // {
    //     int rowsEffected = 0;
    //     string query = "INSERT INTO messages (content, sender_id, conversations_id) " +
    //     "VALUES(@Content, @SenderId, @ConversationId);";
    //     using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
    //     rowsEffected = con.ExecuteScalar<int>(query, param: obj);
    //     return rowsEffected;
    // }
    // public int? Delete(Message message, string testar)
    // {
    //     int rowsEffected = 0;
    //     string query = "UPDATE messages m SET m.is_visible = false " +
    //     "WHERE id = @Id;";
    //     // du ska kunna deleta dina medd. samt dig själv från konversationen
    //     using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
    //     rowsEffected = con.ExecuteScalar<int>(query, new { @id = message.ID });
    //     return rowsEffected;
    // }
    // public List<Message> GetAll(User user, string testar)
    // {
    //     throw new NotImplementedException();
    // }
    // public int? Update(Message obj, string testar)
    // {
    //     string query = "UPDATE messages SET content = @Content WHERE id = @Id;";
    //     using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
    //     int rowsEffected = con.ExecuteScalar<int>(query, param: obj);
    //     return rowsEffected;
    // }
    public List<Message> GetById(int conversationId, User user) ///IDATA
    {
        List<Message> messages = new();
        string query = "SELECT m.content as 'Content', concat(u.first_name, ' ', u.last_name) as 'Sender' " +
       "FROM messages m INNER JOIN conversations c ON m.conversations_id = c.id  " +
       "INNER JOIN users_conversations uc ON c.id = uc.conversations_id " +
       "INNER JOIN users u ON u.id = m.sender_id " +
       "WHERE c.id = @conversationId AND m.is_visible = true GROUP BY m.id " +
       "ORDER BY m.date_created ASC;";
        using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
        messages = con.Query<Message>(query, new { @conversationId = conversationId }).ToList();
        return messages;
    }
}