using WeConnect.Data;

namespace WeConnect.Models;

public class UserService
{
    UsersDB _usersDB;
    CrudDB<User> _crudDB;
    public Action<User> OnDelete;
    public UserService(UsersDB usersDB, CrudDB<User> crudDB)
    {
        _usersDB = usersDB;
        _crudDB = crudDB;
    }

    public int? Create(User user)
    {
        return _crudDB.Create(user, QueryGenerator<User>.InsertQuery(user));
    }

    public List<User> GetBySearch(string name, User user)
    {
        List<User> foundUsers = _usersDB.GetSearches(name);
        List<User> usersAvailable = new();
        foreach (User u in foundUsers)
        {
            User availableUser = _usersDB.GetOne(u.ID, user);
            if (availableUser != null)
            {
                usersAvailable.Add(availableUser);
            }
        }
        return usersAvailable;
    }

    public User GetOne(int id, User user)
    {
        // List<User> allUsers = _userData.GetAll();
        // User user = new();
        // foreach (User item in allUsers)
        // {
        //     if (item.ID == id)
        //     {
        //         user = item;
        //     }
        // }
        try
        {
            User foundUser = _usersDB.GetOne(id, user);
            return foundUser;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    public List<User> GetUsersById(List<int> ids, User user)
    {
        List<User> participants = new();
        foreach (int id in ids)
        {
            User participant = GetOne(id, user);
            participants.Add(participant);
        }
        return participants;
    }

    public int? Remove(User user)
    {
        return _crudDB.Delete(user, QueryGenerator<User>.DeleteQuery(user));
    }

    public int? Update(User user)
    {
        int? rows = _crudDB.Update(user, QueryGenerator<User>.UpdateQuery(user));
        if (rows > 0)
        {
            return rows;
        }
        else
        {
            return null;
        }
    }

    public List<User> GetAll(int data, User user)
    {
        throw new NotImplementedException();
    }

    public int? SetAsDeleted()
    {
        List<User> usersToDelete = _usersDB.GetInactive();
        int usersToDeletedTable = 0;
        // if (usersToDelete == null) throw new InvalidOperationException("No users to delete");
        if (usersToDelete != null)
        {
            foreach (User item in usersToDelete)
            {
                OnDelete?.Invoke(item);
                usersToDeletedTable++;
            }
        }
        return usersToDeletedTable;
    }
}
