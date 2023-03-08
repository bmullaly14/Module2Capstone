using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using TenmoServer.DAO;

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



    }

}

