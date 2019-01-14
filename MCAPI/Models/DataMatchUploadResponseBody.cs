using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Mc.TD.Upload.Domain.DataMatch
{
    public class DataMatchUploadResponseBody
    {
        public ResponseHeader responseheader { get; set; }

        public IList<ResponseDetail> responsedetail { get; set; }
    }
}