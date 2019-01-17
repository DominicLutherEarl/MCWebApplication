using System.ComponentModel.DataAnnotations;

namespace Mc.TD.Upload.Domain.DataMatch
{
    public class Requestheader
    {
        //[Required(ErrorMessage = "orderId field is not present")]
        //[StringLength(maximumLength:int.MaxValue, MinimumLength = 1, ErrorMessage ="Length incorrect")]
        [MCAPIValidation]
        public string orderid { get; set; }

        [Required(ErrorMessage = "ordertype field is not present")]
        public string ordertype { get; set; }//new or existing

        [Required(AllowEmptyStrings = false, ErrorMessage = "businessId field is not present")]
        public string businessid { get; set; }

        [Required(ErrorMessage = "matchtype field is not present")]
        public string matchtype { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "noofrecords field is not present")]
        public string noofrecords { get; set; }

        public string email { get; set; }
    }
}