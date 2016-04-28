﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataContext
{
    public class Site
    {
        public int Id { get; set; }
        public virtual ICollection<Article> Articles { get; set; } 
        public string Name { get; set; }
        public DateTime LastFetched { get; set; }
        public string Url { get; set; }
        public string XpathExpression { get; set; }
        public ParsingStrategy ParsingStrategy { get; set; }
    }

    public enum ParsingStrategy
    {
        NewsSiteMap,
        SiteMap
    }
}
