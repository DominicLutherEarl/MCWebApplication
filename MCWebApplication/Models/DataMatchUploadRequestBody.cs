using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Mc.TD.Upload.Domain.DataMatch
{
    public class DataMatchUploadRequestBody
    {
        [Required]
        public Requestheader requestheader { get; set; }

        [Required]
        public IList<Requestdetail> requestdetail { get; set; }
    }
}