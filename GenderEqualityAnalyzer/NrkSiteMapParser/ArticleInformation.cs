using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NrkSiteMapParser
{
    public class ArticleInformation
    {
        public int SiteId { get; set; }
        public string Url { get; set; }
        public DateTime? LastModified{ get; set; }
    }
}
