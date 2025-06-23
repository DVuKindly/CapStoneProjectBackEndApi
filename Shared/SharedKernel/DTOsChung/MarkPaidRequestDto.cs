using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.DTOsChung
{
    public class MarkPaidRequestDto
    {
        public Guid RequestId { get; set; }
        public string PaymentTransactionId { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public string? PaymentNote { get; set; }
    }
}
