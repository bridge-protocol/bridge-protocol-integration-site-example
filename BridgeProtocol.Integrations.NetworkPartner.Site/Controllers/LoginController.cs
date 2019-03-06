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
            _service = new ServiceWrapper(configuration);
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

                if (response == null)
                    throw new Exception("Error logging in with passport.");

                if (response.MissingClaimTypes.Count > 0)
                    throw new Exception("Missing or invalid required claim types: " + string.Join(",", response.MissingClaimTypes));
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
