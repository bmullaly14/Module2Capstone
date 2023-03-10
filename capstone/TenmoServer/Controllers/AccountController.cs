using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private IUserDao userDao;
        private IAccountDao accountDao;

        public AccountController(IUserDao userDao, IAccountDao accountDao) 
        {
            this.userDao = userDao;
            this.accountDao = accountDao;
        }

        [Authorize(Roles = "admin, user")] // we want to authorize this to only showing balance for userId when user IDs match. not sure if "user" is correct
        [HttpGet("{accountId}")]
        public ActionResult<Account> GetAccountByAccountId(int accountId)
        {
            Account account = null;
            account = accountDao.GetAccountByAccountId(accountId);

            if (account != null)
            {
                return account;
            }
            else
            {
                return NotFound();
            }
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
