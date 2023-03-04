using WeConnect.Data;
namespace WeConnect.Models;
public class LogInService 
{
    LogInDB _logInDB;
    // public Action<int> ActivateOnLogIn;

    public LogInService(LogInDB logInDB)
    {
        _logInDB = logInDB;
    }
    public User LogIn(string email, string passWord)  
    {
        try
        {
            User user = _logInDB.GetMemberByLogIn(email, passWord);
            // ActivateOnLogIn?.Invoke(user.ID);
            _logInDB.UpdateToActivated(user.ID);
            return user;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }
}