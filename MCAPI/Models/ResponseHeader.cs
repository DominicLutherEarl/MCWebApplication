using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mc.TD.Upload.Domain.DataMatch
{
    public class ResponseHeader
    {
        public string orderid { get; set; }
        public List<string> matchStatistics { get; set; }
    }
}