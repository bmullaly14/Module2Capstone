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

        public AccountController(IUserDao userDao, IAccountDao accountDao) { }

        [Authorize(Roles = "user")] // we want to authorize this to only showing balance for userId when user IDs match. not sure if "user" is correct
        [HttpGet("{id}")]
        public ActionResult<Account> GetAccount(int id)
        {
            Account account = accountDao.GetAccountByAccountId(id);

            if (account != null)
            {
                return account;
            }
            else
            {
                return NotFound(); 



            }
        }
    }
}
