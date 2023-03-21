using WeConnect.Data;

namespace WeConnect.Models;

public class UserService
{
    UsersDB _usersDB;
    CrudDB<User> _crudDB;
    PhotoDB _photoDB;
    public Action<User> OnDelete;

    public UserService(UsersDB usersDB, CrudDB<User> crudDB, PhotoDB photoDB)
    {
        _usersDB = usersDB;
        _crudDB = crudDB;
        _photoDB = photoDB;
    }

    public async Task<User> GetUserById(int? id)
    {
        if (id == null)
        {
            return null;
        }
        try
        {
            var user = _usersDB.GetUserById(id);
            var photo = _photoDB.GetProfilePhoto(user);
            user.ProfilePhoto = photo ?? new Photo();
            return user;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    public async Task<int?> Create(User user)
    {
        return _crudDB.Create(user, QueryGenerator<User>.InsertQuery(user));
    }

    public async Task<List<User>> GetBySearch(string name, User user)
    {
        try
        {
            List<User> foundUsers = _usersDB.GetSearches(name);
            List<User> usersAvailable = new();
            foreach (User u in foundUsers)
            {
                User availableUser = _usersDB.GetOne(u.ID, user);

                if (availableUser != null)
                {
                    var photo = _photoDB.GetProfilePhoto(availableUser);
                    availableUser.ProfilePhoto = photo ?? new Photo();
                    usersAvailable.Add(availableUser);
                }
            }
            return usersAvailable;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task<User> GetOne(int id, User user)
    {
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

    public async Task<List<User>> GetUsersById(List<int> ids, User user)
    {
        List<User> participants = new();
        foreach (int id in ids)
        {
            User participant = await GetOne(id, user);
            participants.Add(participant);
        }
        return participants;
    }

    public async Task<int?> Remove(User user)
    {
        return _crudDB.Delete(user, QueryGenerator<User>.DeleteQuery(user));
    }

    public async Task<int?> Update(User user)
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

    public async Task<List<User>> GetAll(int data, User user)
    {
        throw new NotImplementedException();
    }

    public async Task<int?> SetAsDeleted()
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
