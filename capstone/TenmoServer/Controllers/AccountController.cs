using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private IUserDao userDao;
        private IAccountDao accountDao;

        public AccountController(IUserDao userDao, IAccountDao accountDao) 
        {
            this.userDao = userDao;
            this.accountDao = accountDao;
        }

         // we want to authorize this to only showing balance for userId when user IDs match. not sure if "user" is correct
        [HttpGet("{accountId}")]
        public ActionResult<Account> GetAccountByAccountId(int accountId)
        {
            

            string userName = User.Identity.Name;

            User user = userDao.GetUserByName(userName);
            Account userAccount = accountDao.GetAccountByUserId(user.UserId);

            if(userAccount.AccountId == accountId)
            {
                Account account = null;
                account = accountDao.GetAccountByAccountId(accountId);
                if (userAccount != null)
                {
                    return account;
                }
                else
                {
                    return NotFound();
                }

            } else { return Unauthorized(); }

           
        }
        [HttpGet("user/{userId}")]
        public ActionResult<Account> GetAccountByUserId(int userId)
        {
            Account account = null;
           
            account = accountDao.GetAccountByUserId(userId);

            if (account != null)
            {
                return account;
            }
            else
            {
                return NotFound(); 



            }
        }
        [HttpGet("user/name/{username}")]
        public ActionResult<Account> GetAccountByUserName(string username)
        {
            Account account = null;
            User user = userDao.GetUserByName(username);
            account = accountDao.GetAccountByUserId(user.UserId);
            if (account != null)
            {
                return account;
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet("user/{userId}/balance")]
        public ActionResult<decimal> GetAccountBalanceByUserId(int userId)
        {
            Account account = null;

            account = accountDao.GetAccountByUserId(userId);

            if (account != null)
            {
                return account.Balance;
            }
            else
            {
                return NotFound();



            }
        }
        [Authorize(Roles = "admin, user")]

        [HttpGet("/account/{accountId}/balance")]
        public ActionResult<decimal> GetAccountBalance(int accountId)
        {
            Account account = null;
            account = GetAccountByAccountId(accountId).Value;

            if (account != null)
            {
                return account.Balance;
            }
            else
            {
                return NotFound();
            }

        }
    }
}
