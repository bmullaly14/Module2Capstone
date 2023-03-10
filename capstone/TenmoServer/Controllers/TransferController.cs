using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TransferController : Controller
    {

        private ITransferDao transferDao;

        public TransferController(ITransferDao transferDao)
        {
            this.transferDao = transferDao;

        }
        //need HttpGet(ID), HttpPost to transfer to other user,
        //HttpPut(TransactionID) to update both account -- I think we can do this with a post...
        //The actual "doing" will happen in the DAO, so we just have to make the transfer.

        [HttpGet("/transfer/user/{userId}")]
        public ActionResult<IList<Transfer>> GetAllTransfersForUser(int userId)
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

        }
        [HttpGet("/transfer/{transferId}")]
        public ActionResult<Transfer> GetTransferByTransferId(int transferId)
        {
            Transfer transfer = null;

            transfer = transferDao.GetTransfer(transferId);

            if (transfer == null)
            {
                return NotFound();
            }
            else { return transfer; }
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
            Transfer newTransfer = CreateTransfer(transfer).Value;

            if (newTransfer.TransferId == 0)
            {
                return NotFound();
            }

            bool result = transferDao.ExecuteTransfer(newTransfer);

            if (!result)
            {
                return StatusCode(500);
            }
            else
            {
                return newTransfer;
            }

        }

    }
}

        //[Authorize(Roles = "user")]
        //[HttpPost] //create new transfer 



