using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BridgeProtocol.Integrations.NetworkPartner.Site.Models;
using Microsoft.Extensions.Configuration;
using BridgeProtocol.Integrations.Services;
using BridgeProtocol.Integrations.Models;

namespace BridgeProtocol.Integrations.NetworkPartner.Site.Controllers
{
    public class LoginController : Controller
    {
        private ServiceWrapper _service;
        private List<string> _claimTypes = new List<string> { "1","2","3" }; //Require Email address to be provided by the user
        private List<string> _blockchainNetworks = new List<string> { "neo","eth" };

        public LoginController(IConfiguration configuration)
        {
            IConfigurationSection config = configuration.GetSection("Service");
            string serviceUrl = config["Location"];
            string securityHeaderKey = config["SecurityHeaderKey"];
            string securityHeaderValue = config["SecurityHeaderValue"];

            _service = new ServiceWrapper(serviceUrl, securityHeaderKey, securityHeaderValue);
        }

        [HttpGet]
        public IActionResult Index()
        {
            HttpContext.Session.SetString("InitSession", "true");

            var signingToken = Guid.NewGuid().ToString(); //You can use any signing token you want
            return View(new LoginWithBridgePassportViewModel
            {
                BridgePassport_LoginRequest = _service.CreateAuthRequest(signingToken, _claimTypes, _blockchainNetworks),
                SigningToken = signingToken
            });
        }

        [HttpPost]
        public IActionResult Index(LoginWithBridgePassportViewModel model)
        {
            VerifyAuthResponse response = null;

            //Verify we got the passport and it's valid
            try
            {
                response = _service.VerifyAuthResponse(model.BridgePassport_LoginResponse, model.SigningToken);

                //Make sure we got a valid response
                if (response == null)
                    throw new Exception("Error logging in with passport.");

                //Evaluate response.claims to ensure they are valid and provided what is required
                //Evaluate response.BlockchainAddresses to ensure the requested addresses were provided

                //Get the passport info
                var passportDetails = _service.GetPassportDetails(response.PassportId);

                //Verify the passport isn't blacklisted
                if (passportDetails.IsBlacklisted)
                    throw new Exception("Passport is blacklisted.");

                HttpContext.Session.SetString("PassportPublicKey",response.PublicKey);
                HttpContext.Session.SetString("PassportId", response.PassportId);
            }
            catch(Exception ex)
            {
                return new ObjectResult("Error logging in with passport: " + ex.Message);
            }

           
            //Get the claim types enum so we can provide the names in the UI
            ViewData["ClaimTypes"] = _service.GetClaimTypes();

            return View("LoginResponse", response);
        }
    }
}
