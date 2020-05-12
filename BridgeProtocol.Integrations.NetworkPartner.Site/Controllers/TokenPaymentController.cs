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
            IConfigurationSection config = configuration.GetSection("Service");
            string serviceUrl = config["Location"];
            string securityHeaderKey = config["SecurityHeaderKey"];
            string securityHeaderValue = config["SecurityHeaderValue"];

            _service = new ServiceWrapper(serviceUrl, securityHeaderKey, securityHeaderValue);
        }

        // GET: TokenPayment
        public IActionResult Index()
        {
            var passportId = _service.GetServerPassportId();
            var network = "neo";
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
                BridgeProtocol_PaymentRequest = _service.CreatePaymentRequest(network, amount, address, identifier)
            });
        }

        // POST: TokenPayment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(TokenPaymentViewModel model)
        {
            var res = _service.VerifyPaymentResponse(model.BridgeProtocol_PaymentResponse);
            if (res != null)
                model.BridgeProtocol_PaymentVerified = true;

            return View(model);
        }
    }
}