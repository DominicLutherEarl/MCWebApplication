using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace Mc.TD.Upload.Domain.DataMatch
{
    public class DataMatchUploadRequestBody
    {
        [Required(ErrorMessage ="Request header is null")]
        public Requestheader requestheader { get; set; }

        [Required(ErrorMessage ="Request Detail is null")]
        public IList<Requestdetail> requestdetail { get; set; }
    }
}