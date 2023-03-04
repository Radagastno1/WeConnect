using WeConnect.Data;

namespace WeConnect.Models;

public class BlockingService
{
    public Func<User, int, int> OnBlockUser;
    BlockingsDB _blockingsDB;
    public BlockingService(BlockingsDB blockingsDB)
    {
        _blockingsDB = blockingsDB;
    }

    public int Create(User user, int id)
    {
        int blockedId = 0;
        if (user.ID != id)
        {
            try
            {
                OnBlockUser?.Invoke(user, id);
                blockedId = _blockingsDB.Create(user, id);
            }
            catch (InvalidOperationException)
            {
                blockedId = 0;
            }
        }
        return blockedId;
    }

    public List<User> GetMine(User user)
    {
        try
        {
            return _blockingsDB.GetMine(user);
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    public int Delete(User obj, int id) //unblock user
    {
        throw new NotImplementedException();
    }

    public void Update(User obj)
    {
        throw new NotImplementedException();
    }
}
