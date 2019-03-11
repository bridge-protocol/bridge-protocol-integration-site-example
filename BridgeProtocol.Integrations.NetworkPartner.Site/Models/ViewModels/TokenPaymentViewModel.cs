using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BridgeProtocol.Integrations.NetworkPartner.Site.Models.ViewModels
{
    public class TokenPaymentViewModel
    {
        public string BridgeProtocol_PaymentIdentifier { get; set; }
        public int BridgeProtocol_PaymentAmount { get; set; }
        public string BridgeProtocol_PaymentAddress { get; set; }
        public string BridgeProtocol_PaymentTransactionId { get; set; }
    }
}
