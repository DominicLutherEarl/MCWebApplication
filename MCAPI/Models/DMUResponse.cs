using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace Mc.TD.Upload.Domain.DataMatch
{
    public class DMUResponse
    {
        public DMUResponse()
        {
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string orderId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<ErrorData> errorData { get; set; }
    }
}