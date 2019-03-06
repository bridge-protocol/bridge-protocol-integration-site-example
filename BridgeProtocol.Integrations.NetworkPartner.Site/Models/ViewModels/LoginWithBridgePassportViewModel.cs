using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BridgeProtocol.Integrations.NetworkPartner.Site.Models
{
    public class LoginWithBridgePassportViewModel
    { 
        public string SigningToken { get; set; }
        public string BridgePassport_LoginRequest { get; set; }
        public string BridgePassport_LoginResponse { get; set; }
    }
}
 