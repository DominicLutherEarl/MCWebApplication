using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Mc.TD.Upload.Domain.DataMatch
{
    public class DataMatchUploadRequest
    {
        public Header RequestHeader { get; set; }

        public IList<Detail> RequestDetails { get; set; }
    }
}