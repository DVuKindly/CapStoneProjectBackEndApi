using System.Collections.Generic;

namespace PaymentService.API.DTOs.Requests
{
    public class VnPayWebhookDto
    {
        public string vnp_TxnRef { get; set; }
        public string vnp_TransactionNo { get; set; }
        public string vnp_SecureHash { get; set; }
        public string vnp_ResponseCode { get; set; }

        public Dictionary<string, string> RawData { get; set; } = new();
    }
}
