using Microsoft.AspNetCore.Mvc.Rendering;

namespace WeConnect.Models;

[Serializable]
public class ProfileViewModel
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public string BirthDate { get; set; }
    public string Gender { get; private set; }

    public string AboutMe { get; set; }
    public List<User> MyFriends { get; set; } = new();
}
