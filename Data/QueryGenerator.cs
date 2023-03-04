using WeConnect.Models;
namespace WeConnect.Data;
public class QueryGenerator<T>
{
    public static string InsertQuery(T obj)
    {
        string query = string.Empty;
        switch (obj)
        {
            case User:
            query =  "INSERT INTO users(first_name, last_name, email, pass_word, birth_date, gender, about_me, role_id) " +
       "VALUES(@FirstName, @LastName, @Email, @PassWord, @BirthDate, @Gender, @AboutMe, 5); " +
       "SELECT LAST_INSERT_ID();";
                break;
            case Message:
        query = "INSERT INTO messages (content, sender_id, conversations_id) " +
        "VALUES(@Content, @SenderId, @ConversationId);";
                break;
            case Conversation:
           query =  "INSERT INTO conversations(creator_id) VALUES(@CreatorId);" +
        "SELECT LAST_INSERT_ID();";
                break;
            case Comment:
           query = "INSERT INTO posts (content, users_id, post_type, on_post_id) " +
         "VALUES (@Content, @UserId, 'Comment', @OnPostId);";
                break;
            case Post:
            query = "INSERT INTO posts (content, users_id, post_type) " +
        "VALUES(@Content, @UserId, 'Post'); SELECT LAST_INSERT_ID()";
                break;
        }
        return query;
    }
    public static string SelectQuery(T obj, User user)
    {
        string query = string.Empty;
        switch (obj)
        {
            case User:
            query = "SELECT u.id, u.first_name as 'FirstName', u.last_name as 'LastName', u.email," +
      "u.pass_word as 'PassWord', DATE_FORMAT(u.birth_date, '%Y-%m-%d') as 'BirthDate', u.gender, u.about_me as 'AboutMe', r.name as 'Role' " +
         "FROM users u " +
         "INNER JOIN roles r ON r.id = ur.roles_id WHERE u.is_deleted = false;";
                break;
            case Message:
                break;
            case Conversation:
            query = "SELECT uc.conversations_id as 'Id', c.date_created as 'DateCreated', c.creator_id as 'CreatorId', u.id as 'ParticipantId'" +
                        "FROM conversations c " +
                        "INNER JOIN users_conversations uc " +
                        "ON c.id = uc.conversations_id " +
                        "INNER JOIN users u " +
                         "ON u.id = uc.users_id " +
                         "WHERE u.is_active = true " +
                       "AND u.id = @Id;";
                break;
            case Comment:
            query = "SELECT p.id, p.content, p.date_created as 'DateCreated', p.users_id as 'UserId', " +
         "p.on_post_id as 'OnPostId', u.first_name as 'FirstName', u.last_name as 'LastName' FROM posts p " +
         "INNER JOIN users u ON p.users_id = u.id " +
         "WHERE p.post_type = 'Comment' AND p.is_visible = TRUE AND p.users_id = @Id;";
                break;
            case Post:
            query = "SELECT p.id as 'Id', p.content as 'Content', p.date_created as 'DateCreated', " +
        "u.first_name as 'FirstName', u.last_name as 'LastName', p.users_id as 'UserId' " +
        "FROM posts p INNER JOIN users u " +
        "ON p.users_id = u.id " +
        "INNER JOIN users_friends uf1 " +
        "ON u.id = uf1.users_id1 " +
        "WHERE p.post_type = 'Post' " +
        "AND p.is_visible = TRUE " +
        "AND p.is_deleted = FALSE " +
        "AND uf1.users_id2 = @Id;";
                break;
        }
        return query;
    }
    public static string UpdateQuery(T obj)
    {
        string query = string.Empty;
        switch (obj)
        {
            case User:
             query = "Update users SET first_name = @FirstName, last_name = @LastName, " +
         "email = @Email, pass_word = @PassWord, gender = @Gender, " +
         "about_me = @AboutMe WHERE id = @ID;";
                break;
            case Message:
             query = "UPDATE messages SET content = @Content WHERE id = @Id;";
                break;
            case Conversation:
            query = "INSERT INTO users_conversations(users_id, conversations_id) VALUES (@participantId, @Id);" +
        "SELECT LAST_INSERT_ID();";
                break;
            case Comment:
            query = "UPDATE posts SET content = @Content WHERE id = @Id;";
                break;
            case Post:
            query = "UPDATE posts SET content = @Content, is_edited = True WHERE id = @Id;";
                break;
        }
        return query;
    }
    public static string DeleteQuery(T obj)
    {
        string query = string.Empty;
        switch (obj)
        {
            case User:
              query = "START TRANSACTION;" +
        "UPDATE users SET is_active = false WHERE id = @id;" +
         "UPDATE messages SET is_visible = FALSE WHERE sender_id = @id;" +
         "UPDATE posts SET is_visible = FALSE WHERE users_id = @id;" +
         "COMMIT;";
                break;
            case Message:
            query = "UPDATE messages m SET m.is_visible = false " +
    "WHERE id = @Id;";
                break;
            case Conversation:
                break;
            case Comment:
            query = "UPDATE posts SET is_visible = FALSE WHERE id = @Id AND on_post_id = @OnPostId;";
                break;
            case Post:
            query = "Update posts SET is_deleted = TRUE WHERE id = @Id;";
                break;
        }
        return query;
    }
}