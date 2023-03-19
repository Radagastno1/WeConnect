using WeConnect.Data;

namespace WeConnect.Models;
public class PhotoService
{
    PhotoDB _photoDB;

    public PhotoService(PhotoDB photoDB)
    {
        _photoDB = photoDB;
    }
    public string UpdateProfilePhoto(User user, string imageUrl)
    {
        return _photoDB.UpdateProfilePhoto(user, imageUrl);
    }
}