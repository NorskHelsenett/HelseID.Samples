using System;

namespace HelseId.Core.MVCHybrid.ClientAuthenticationAPIAccess.Sample.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
