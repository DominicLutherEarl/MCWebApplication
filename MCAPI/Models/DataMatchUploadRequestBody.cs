using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Mc.TD.Upload.Domain.DataMatch
{
    public class DataMatchUploadRequestBody
    {
        public Requestheader requestheader { get; set; }

        public IList<Requestdetail> requestdetail { get; set; }
    }
}