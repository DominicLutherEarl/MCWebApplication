using System.ComponentModel.DataAnnotations;

namespace Mc.TD.Upload.Domain.DataMatch
{
    public class Requestheader
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "orderId field is not present")]
        [StringLength(maximumLength:10, MinimumLength = 8, ErrorMessage ="Length incorrect")]
        public string orderid { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "ordertype field is not present")]
        public string ordertype { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "businessId field is not present")]
        public string businessid { get; set; }

        public string matchtype { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "noofrecords field is not present")]
        public string noofrecords { get; set; }

        public string email { get; set; }
    }
}