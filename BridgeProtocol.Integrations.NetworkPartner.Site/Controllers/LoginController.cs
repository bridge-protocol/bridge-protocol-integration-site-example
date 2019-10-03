using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using BridgeProtocol.Integrations.NetworkPartner.Site.Models;
using Microsoft.Extensions.Configuration;
using BridgeProtocol.Integrations.Services;
using BridgeProtocol.Integrations.Models;

namespace BridgeProtocol.Integrations.NetworkPartner.Site.Controllers
{
    public class LoginController : Controller
    {
        private ServiceWrapper _service;
        private List<int> _claimTypes = new List<int> { 3 }; //Require Email address to be provided by the user
        private int? _profileTypeId; // = 3;

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
            var signingToken = Guid.NewGuid().ToString(); //You can use any signing token you want
            return View(new LoginWithBridgePassportViewModel
            {
                BridgePassport_LoginRequest = _service.CreatePassportLoginChallengeRequest(signingToken, _profileTypeId, _claimTypes),
                SigningToken = signingToken
            });
        }

        [HttpPost]
        public IActionResult Index(LoginWithBridgePassportViewModel model)
        {
            VerifyPassportLoginChallengeResponse response = null;

            //Verify we got the passport and it's valid
            try
            {
                response = _service.VerifyPassportLoginChallengeResponse(model.BridgePassport_LoginResponse, model.SigningToken, _claimTypes);

                //Make sure we got a valid response
                if (response == null)
                    throw new Exception("Error logging in with passport.");

                //Make sure we aren't missing any claim types we asked for
                if (response.MissingClaimTypes.Count > 0)
                    throw new Exception("Missing or invalid required claim types: " + string.Join(",", response.MissingClaimTypes));

                //Verify all the claims we received were signed by known verification partners on the bridge network
                if (response.UnknownSignerClaimTypes.Count > 0)
                    throw new Exception("One or more claims were signed by an unknown signer.");

                //Verify the passport isn't blacklisted
                if (response.PassportDetails.IsBlacklisted)
                    throw new Exception("Passport is blacklisted.");
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
