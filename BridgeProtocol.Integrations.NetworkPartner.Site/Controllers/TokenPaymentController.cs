using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BridgeProtocol.Integrations.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BridgeProtocol.Integrations.NetworkPartner.Site.Controllers
{
    public class TokenPaymentController : Controller
    {
        private string _paymentIdentifier;
        private ServiceWrapper _service;

        public TokenPaymentController(IConfiguration configuration)
        {
            _service = new ServiceWrapper(configuration);
        }

        // GET: TokenPayment
        public IActionResult Index()
        {
            _paymentIdentifier = Guid.NewGuid().ToString();
            return View();
        }

        // POST: TokenPayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                return new OkObjectResult("Payment Sent on NEO, Tx: " + collection["BridgePassport_PaymentTransactionId"]);
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public IActionResult GetTransactionStatus(string transactionId)
        {
            var complete = _service.CheckBlockchainTransactionComplete("NEO", transactionId);
            var details = _service.GetBlockchainTransactionDetails("NEO", transactionId);
            return new ObjectResult(details);
        }
    }
}