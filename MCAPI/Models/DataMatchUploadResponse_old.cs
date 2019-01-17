using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace Mc.TD.Upload.Domain.DataMatch
{
    public class DataMatchUploadResponse_old
    {
        public DataMatchUploadResponse_old()
        {
        }

        public string result { get; set; }

        public int statusCode { get; set; }

        public string status { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string errorExplanation { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string errorField { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string errorValidationType { get; set; }
    }
}