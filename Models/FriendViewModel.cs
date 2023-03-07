using Microsoft.AspNetCore.Mvc.Rendering;

namespace WeConnect.Models;

[Serializable]
public class FriendViewModel
{
    public int ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string BirthDate { get; set; }
    public string Gender { get;  set; }
     public string AboutMe { get; set; }
}
