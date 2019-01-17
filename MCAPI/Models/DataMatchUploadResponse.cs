using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Mc.TD.Upload.Domain.DataMatch
{
    public class DataMatchUploadResponse
    {
        public Header ResponseHeader { get; set; }

        public IList<Detail> ResponseDetails { get; set; }
    }
}