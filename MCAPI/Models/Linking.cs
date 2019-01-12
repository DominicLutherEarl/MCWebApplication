using System.Collections.Generic;

namespace Mc.TD.Upload.Domain.DataMatch
{
    public class Linking
    {
        public string linktrackid { get; set; }
        public IList<Linkcompliance> linkcompliance { get; set; }
    }
}