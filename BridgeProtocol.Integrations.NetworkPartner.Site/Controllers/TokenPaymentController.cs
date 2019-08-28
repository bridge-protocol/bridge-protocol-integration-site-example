using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BridgeProtocol.Integrations.NetworkPartner.Site.Models.ViewModels;
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
            var passportId = _service.GetServerPassportId();
            var network = "NEO";
            var amount = 1;
            var address = "ALEN8KC46GLaadRxaWdvYBUhdokT3RhxPC";
            var identifier = Guid.NewGuid().ToString();

            return View(new TokenPaymentViewModel
            {
                BridgeProtocol_PassportId = passportId,
                BridgeProtocol_PaymentNetwork = network,
                BridgeProtocol_PaymentAddress = address,
                BridgeProtocol_PaymentAmount = amount,
                BridgeProtocol_PaymentIdentifier = identifier,
                BridgeProtocol_PaymentRequest = _service.CreatePassportPaymentRequest(network, amount, address, identifier)
            });
        }

        // POST: TokenPayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(TokenPaymentViewModel model)
        {
            var res = _service.VerifyPassportPaymentResponse(model.BridgeProtocol_PaymentResponse);
            return View(model);
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