using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mc.TD.Upload.Domain.DataMatch
{
    public class Detail
    {
        public string id { get; set; }
        public string requesttype { get; set; }
        public string trackid { get; set; }
        public string companyname { get; set; }
        public Address address { get; set; }
        public string phone { get; set; }
        public string url { get; set; }
        public string contact { get; set; }
        public string ein { get; set; }
        public string tin { get; set; }
        public string vat { get; set; }
        public string registrationnumber { get; set; }
        public string monitoringType { get; set; }
        public string updatetype { get; set; }
        public Linking linking { get; set; }
        public CustomFields customfields { get; set; }
        public List<ErrorData> errorData { get; set; }
    }
}