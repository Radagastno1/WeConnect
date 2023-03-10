using System.ComponentModel;

namespace WeConnect.Models;

[Serializable]
public class User
{
    public int ID { get; private set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PassWord { get; set; }
    public string BirthDate { get; set; }
    public string Gender { get; private set; }
    public Roles Role { get; private set; }
    public string AboutMe { get; set; }
    public List<User> MyFriends { get; set; } = new();
    public int FriendStatus { get; set; }
    public Photo ProfilePhoto{get;set;}

    public User() { }

    public User(
        string aFirstName,
        string aLastName,
        string aEmail,
        string aPassWord,
        string aBirthDate,
        string aGender
    )
    {
        FirstName = aFirstName;
        LastName = aLastName;
        Email = aEmail;
        PassWord = aPassWord;
        BirthDate = aBirthDate;
        Gender = aGender;
    }

    public void SetRole(int enumNumber)
    {
        if (Enum.IsDefined(typeof(Roles), enumNumber))
        {
            Role = (Roles)enumNumber;
        }
    }

    public override string ToString()
    {
        return $"[{ID}]  {FirstName} {LastName}";
    }

    public enum Genders
    {
        Undecided,
        Woman,
        Man,
        Agender,
        Bigender,
        Nonbinary
    }

    public enum Roles //ej använt mycket av roles varken utnyttjat users_roles eller i c#, för utveckling får det bli
    {
        Undecided,
        Member,

        [Description("Customer Service")]
        CustomerService,
        Editor,
        Administrator,

        [Description("Super Admin")]
        SuperAdmin
    }
}
