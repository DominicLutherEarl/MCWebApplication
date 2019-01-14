using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace Mc.TD.Upload.Domain.DataMatch
{
    public class ErrorData
    {
        public ErrorData()
        {
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string errorCause { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string errorExplanation { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string errorField { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string errorValidationType { get; set; }
    }
}