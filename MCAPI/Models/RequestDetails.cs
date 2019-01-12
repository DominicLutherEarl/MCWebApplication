using System.ComponentModel.DataAnnotations;

namespace Mc.TD.Upload.Domain.DataMatch
{
    public class Requestdetail
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Id field is not present")]
        public string id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "requesttype field is not present")]
        public string requesttype { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "trackId field is not present")]
        public string trackid { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "companyname field is not present")]
        public string companyname { get; set; }

        public Address address { get; set; }
        public string phone { get; set; }
        public string url { get; set; }
        public string contact { get; set; }
        public string ein { get; set; }
        public string tin { get; set; }
        public string vat { get; set; }
        public string registrationnumber { get; set; }
        public string updatetype { get; set; }
        public Linking linking { get; set; }
        public CustomFields customfields { get; set; }
    }
}