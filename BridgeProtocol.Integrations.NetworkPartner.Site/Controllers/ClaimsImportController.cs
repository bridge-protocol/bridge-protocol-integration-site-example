using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BridgeProtocol.Integrations.Models;
using BridgeProtocol.Integrations.NetworkPartner.Site.Models.ViewModels;
using BridgeProtocol.Integrations.Services;
using BridgeProtocol.Integrations.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BridgeProtocol.Integrations.NetworkPartner.Site.Controllers
{
    public class ClaimsImportController : Controller
    {
        private ServiceWrapper _service;

        public ClaimsImportController(IConfiguration configuration)
        {
            IConfigurationSection config = configuration.GetSection("Service");
            string serviceUrl = config["Location"];
            string securityHeaderKey = config["SecurityHeaderKey"];
            string securityHeaderValue = config["SecurityHeaderValue"];

            _service = new ServiceWrapper(serviceUrl, securityHeaderKey, securityHeaderValue);
        }
        // GET: ClaimsImport
        public ActionResult Index()
        {
            var publicKey = HttpContext.Session.GetString("PassportPublicKey");
            if (String.IsNullOrEmpty(publicKey))
                throw new Exception("Passport public key mmissing. User must be logged in with passport first.");

            var claims = new List<Claim>();
            var claim = new Claim
            {
                ClaimTypeId = "3",
                ClaimValue = "someuser@someplace.com",
                CreatedOn = DateTimeUtility.GetUnixTime(DateTime.UtcNow),
                ExpiresOn = 0
            };
            claims.Add(claim);
            return View(new ClaimsImportViewModel
            {
                ClaimsImportRequest = _service.CreateClaimsImportRequest(publicKey, claims)
            }); 
        }
    }
}