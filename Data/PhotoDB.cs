using WeConnect.Models;
using Dapper;
using MySqlConnector;

namespace WeConnect.Data;

public class PhotoDB
{
    public Photo GetProfilePhoto(User user)
    {
        string query =
            "SELECT p.id, p.image_url AS 'ImageURL' FROM photos p " +
            "INNER JOIN photo_album pa " + 
            "ON pa.id = p.photo_album_id " + 
            "INNER JOIN users u " + 
            "ON u.id = pa.users_id " + 
            "WHERE u.id = @id AND u.profile_photo_id = p.id";
        try
        {
            using MySqlConnection con =
                new($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
            var photo = con.QuerySingle<Photo>(query, new { @id = user.ID });
            return photo;
        }
        catch { 
            return null;
        }
    }
}
