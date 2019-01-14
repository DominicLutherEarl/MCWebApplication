using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Mc.TD.Upload.Domain.DataMatch
{
    public class ResponseDetail
    {
        public string id { get; set; }
        public IList<Requestdetail> requestData { get; set; }
        public List<ErrorData> errorData { get; set; }
    }
}