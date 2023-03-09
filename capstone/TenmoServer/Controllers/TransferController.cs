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
            } else { return transfer; }
        }

        [HttpPost()]
        public ActionResult<Transfer> ExecuteSendTransfer(Transfer transfer)
        {
            Transfer newTransfer = null;
            newTransfer = transferDao.ExecuteTransfer(transfer);

            transfer = transferDao.GetTransfer(newTransfer.TransferId);

            if(transfer.TransferId != 0)
            {
                return transfer;
            } else if (newTransfer == null)
            {
                return NotFound();
            } else
            {
                return StatusCode(500);
            }
             
        }        
        
    }

}

