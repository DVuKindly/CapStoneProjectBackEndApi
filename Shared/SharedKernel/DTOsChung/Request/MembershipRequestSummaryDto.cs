using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.DTOsChung.Request
{
    public class MembershipRequestSummaryDto
    {
        public Guid MembershipRequestId { get; set; }  // 🔥 Bổ sung dòng này
        public Guid AccountId { get; set; }
        public Guid? PackageId { get; set; }
        public string? RequestedPackageName { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = null!;
    }
}
