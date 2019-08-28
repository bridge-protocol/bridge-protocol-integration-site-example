using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BridgeProtocol.Integrations.NetworkPartner.Site.Models.ViewModels
{
    public class TokenPaymentViewModel
    {
        public string BridgeProtocol_PassportId { get; set; }
        public string BridgeProtocol_PaymentNetwork { get; set; }
        public string BridgeProtocol_PaymentIdentifier { get; set; }
        public int BridgeProtocol_PaymentAmount { get; set; }
        public string BridgeProtocol_PaymentAddress { get; set; }
        public string BridgeProtocol_PaymentRequest { get; set; }
        public string BridgeProtocol_PaymentResponse { get; set; }
    }
}
