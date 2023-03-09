using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("[controller")]
    [ApiController]
    public class TransferController : Controller
    {
        private ITransferDao transferDao;

        public TransferController(ITransferDao transferDao)
        {
            this.transferDao = transferDao;

        }
        //need HttpGet(ID), HttpPost to transfer to other user, HttpPut(TransactionID) to update both account

        [HttpGet("/transfer/user/{userId}")]
        public ActionResult<IList<Transfer>> GetAllTransfersForUser(int userId)
        {
            List<Transfer> transfers = new List<Transfer>();

            transfers = (List<Transfer>)transferDao.GetTransfersByUserId(userId);

            if(transfers.Count != 0)
            {
                return transfers;
            }
            else
            {
                return NoContent();
            }

        }
        [HttpGet("/transfer/{transferId}")]
        public ActionResult<Transfer> GetTransferByTransferId(int transferId)
        {
            Transfer transfer = null;

            transfer = transferDao.GetTransfer(transferId);

            if(transfer == null)
            {
                return NotFound();
            } else { return transfer; }
        }

        [HttpPost()]
        public ActionResult<Transfer> ExecuteSendTransfer(Transfer transfer)
        {
            transferDao.ExecuteTransfer(transfer);
            return new Transfer();
        }
        [HttpPut("{transId")]

    }

}

