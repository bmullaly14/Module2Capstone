using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class TransferController : ControllerBase
    {

        private ITransferDao transferDao;
        private IUserDao userDao;
        private IAccountDao accountDao;

        public TransferController(ITransferDao transferDao, IUserDao userDao, IAccountDao accountDao)
        {
            this.transferDao = transferDao;
            this.userDao = userDao;
            this.accountDao = accountDao;

        }
        //need HttpGet(ID), HttpPost to transfer to other user,
        //HttpPut(TransactionID) to update both account -- I think we can do this with a post...
        //The actual "doing" will happen in the DAO, so we just have to make the transfer.

        [HttpGet("/transfer/user/{userId}")]
        public ActionResult<IList<Transfer>> GetAllTransfersForUser(int userId)
        {

            if (CheckUser(userId).Value)
            {
                List<Transfer> transfers = new List<Transfer>();

                transfers = (List<Transfer>)transferDao.GetTransfersByUserId(userId);

                if (transfers == null)
                {
                    return NoContent();
                }
                else
                {
                    return transfers;
                }
            } else { return Unauthorized(); }

        }
        [HttpGet("/transfer/{transferId}")]
        public ActionResult<Transfer> GetTransferByTransferId(int transferId)
        {
            Transfer transfer = null;

            transfer = transferDao.GetTransfer(transferId);

            if (CheckAuthForTransfer(transfer).Value)
            {

                if (transfer == null)
                {
                    return NotFound();
                }
                else { return transfer; }
            } else { return Unauthorized(); }
        }
        //Create Transfer will be HttpPost, take in Transfer, put out Transfer w/ ID from SQL DB
       [HttpPost]
        public ActionResult<Transfer> CreateTransfer(Transfer transfer)
        {
            Transfer newTransfer = transferDao.CreateTransfer(transfer);

            return Created($"/transfer/{newTransfer.TransferId}", newTransfer);
        }

        [HttpPut]
        public ActionResult<Transfer> ExecuteSendTransfer(Transfer transfer) // take in a transfer FROM create transfer in DAO
        {            
            //Transfer newTransfer = (Transfer)CreateTransfer(transfer).Value;

            if(transfer.TransferId == 0)
            {
                return NotFound();
            }

            bool result = transferDao.ExecuteTransfer(transfer);            

            if (!result)
            {
                return StatusCode(500);
            }
            else
            {
                return transfer;
            }
             
        } 
        public ActionResult<bool> CheckUser(int userId)
        {
            string userName = User.Identity.Name;

            User user = userDao.GetUserByName(userName);            
            
            if (user.UserId == userId)
            {
                return true;
            } else { return false; }
        }
        public ActionResult<bool> CheckAuthForTransfer(Transfer transfer)
        {
            if(CheckUser(accountDao.GetAccountByAccountId(transfer.AccountFrom).UserId).Value || CheckUser(accountDao.GetAccountByAccountId(transfer.AccountTo).UserId).Value)
            {
                return true;
            } else { return false; }
        }
    }
}

        //[Authorize(Roles = "user")]
        //[HttpPost] //create new transfer 

}

    
