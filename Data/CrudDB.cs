using Dapper;
using MySqlConnector;
using WeConnect.Models;
namespace WeConnect.Data;
public class CrudDB<T>
{
    public int? Create(T obj, string query)
    {
        try
        {
            using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
            int num = con.ExecuteScalar<int>(query, param: obj);
            return num;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }
    public int? Delete(T obj, string query)
    {
        int rowsEffected = 0;
        try
        {
            using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
            rowsEffected = con.ExecuteScalar<int>(query, param: obj);
            return rowsEffected;
        }
        catch (InvalidOperationException)
        {
            return rowsEffected;
        }
    }
    public List<T> GetAll(User user, string query)
    { //param obj är tex för att kolla mot user (id) som kommer in som two obj  ev göra till user bara?
        try
        {
            using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
            List<T> objects = con.Query<T>(query, param: user).ToList();
            return objects;
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }
    public int? Update(T obj, string query)
    {
        try
        {
            using MySqlConnection con = new MySqlConnection($"Server=localhost;Database=facebook_lite;Uid=root;Pwd=;");
            int rowsEffected = con.ExecuteScalar<int>(query, param: obj);
            return rowsEffected;
        }
        catch(InvalidOperationException)
        {
            return null;
        }
    }
}