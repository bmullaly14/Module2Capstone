using System.Collections.Generic;
using TenmoServer.Models;
namespace TenmoServer.DAO
{
    public interface IAccountDao
    {
        Account GetAccountByAccountId(int accountId);
        Account GetAccountByUserId(int userId);
        //Account AddAccountToUser(int userId);
    }
}
