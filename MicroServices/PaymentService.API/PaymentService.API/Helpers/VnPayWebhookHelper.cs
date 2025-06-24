using Microsoft.AspNetCore.Http;
using PaymentService.API.DTOs.Requests;
using System.Collections.Generic;

namespace PaymentService.API.Helpers
{
    public static class VnPayWebhookHelper
    {
        public static VnPayWebhookDto ToVnPayWebhookDto(IQueryCollection query)
        {
            var dto = new VnPayWebhookDto();
            var rawData = new Dictionary<string, string>();

            foreach (var kv in query)
            {
                var key = kv.Key;
                var value = kv.Value.ToString();

                rawData[key] = value;

                switch (key)
                {
                    case "vnp_TxnRef":
                        dto.vnp_TxnRef = value;
                        break;
                    case "vnp_TransactionNo":
                        dto.vnp_TransactionNo = value;
                        break;
                    case "vnp_SecureHash":
                        dto.vnp_SecureHash = value;
                        break;
                    case "vnp_ResponseCode":
                        dto.vnp_ResponseCode = value;
                        break;
                }
            }

            dto.RawData = rawData;
            return dto;
        }
    }
}
