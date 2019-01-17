using System.ComponentModel.DataAnnotations;

namespace Mc.TD.Upload.Domain.DataMatch
{
    public class Requestheader
    {
        public string orderid { get; set; }
        public string ordertype { get; set; }//new or existing
        public string businessid { get; set; }
        public string matchtype { get; set; }
        public string noofrecords { get; set; }
        public string email { get; set; }
    }
}