using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TenmoServer.DAO;

namespace TenmoServer.Controllers
{
    [Route("[controller")]
    [ApiController]
    public class BalanceController : Controller
    {
        private IUserDao userDao;

        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "user")] // we want to authorize this to only showing balance for userId when user IDs match. not sure if "user" is correct
        [HttpGet("{id}")]
        public ActionResult<Balance> GetBalance(int id)
        {
            Balance balance = userDao.Get(id);

            if (balance != null)
            {
                return balance;
            }
            else
            {
                return NotFound();
            }
        }
    }
}
