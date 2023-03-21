using WeConnect.Data;

namespace WeConnect.Models;

public class FriendService
{
    FriendsDB _friendsDB;
    PhotoDB _photoDB;
    Action<User>UpdatingFriendStatus;
    UserService _userService;

    public FriendService(FriendsDB friendsDB, PhotoDB photoDB, UserService userService)
    {
        _friendsDB = friendsDB;
        _photoDB = photoDB;
        _userService = userService;
        UpdatingFriendStatus += Update;
    }

    public int Create(User user, int friendId)
    {
        try
        {
            _friendsDB.Create(user, friendId);
            UpdatingFriendStatus?.Invoke(user);
            return 1;
        }
        catch (InvalidOperationException)
        {
            return 0;
        }
    }

    public bool IsBefriended(User user, int friendId)
    {
        try
        {
            if (_friendsDB.CheckIfBefriended(user, friendId) > 0)
            {
                return true;
            }
            else
                return false;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }

    public bool IsFriends(User user, int friendId)
    {
        try
        {
            if (_friendsDB.CheckIfFriend(user, friendId) > 0)
            {
                return true;
            }
            else
                return false;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }

    public void Update(User user)
    {
        List<int> friendRequestsIds = _friendsDB.GetMyFriendRequests(user);
        List<int> friendsToBeAccepted = new();
        foreach (int id in friendRequestsIds)
        {
            if (_friendsDB.CheckIfFriendAccepted(user, id) < 1)
            {
                break;
            }
            else
            {
                friendsToBeAccepted.Add(id);
            }
        }
        foreach (int id in friendsToBeAccepted)
        {
            _friendsDB.Update(user, id);
        }
    }

    public bool IsFriendRequestWaiting(User user, int friendId)
    {
        try
        {
            if (_friendsDB.CheckIfFriendAccepted(user, friendId) > 0)
                return true;
            else
                return false;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }

    public async Task<List<User>> GetMine(User user)
    {
        try
        {
            var friends = _friendsDB.GetMine(user);
            List<User> friendsToReturn = new();
            foreach(var friend in friends)
            {
                friend.ProfilePhoto = _photoDB.GetProfilePhoto(friend);
            }
            return friends;
        }
        catch (InvalidOperationException)
        {
            List<User> users = new();
            return users;
        }
    }

    public async Task LoadFriends(User user)
    {
        user.MyFriends.Clear();
        user.MyFriends = await GetMine(user);
    }

    public int Delete(User user, int friendId)
    { //FELHANTERA
        return _friendsDB.Delete(user, friendId);
    }
}
