using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mc.TD.Upload.Domain.DataMatch
{
    public class Header
    {
        public string orderid { get; set; }
        public string ordertype { get; set; }
        public string businessid { get; set; }
        public string matchtype { get; set; }
        public string noofrecords { get; set; }
        public string email { get; set; }
        public matchStatistics matchStatistics { get; set; }
        public List<ErrorData> errorData;
    }
    public class matchStatistics
    {
        public int totalRecords { get; set; }
        public int errorRecords { get; set; }
    }
}