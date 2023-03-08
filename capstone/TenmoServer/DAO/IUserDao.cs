using System.Collections.Generic;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public interface IUserDao
    {
        User GetUser(int userId);
        User AddUser(string username, string password);
        List<User> GetUsers();
    }
}
