using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Core.DataContext;
using Core.Repositories;


namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            var siteRepo = new SiteRepository();

            foreach (var site in siteRepo.Get().Where(x => x.Name == "Nettavisen"))
            {
                var parser = ImageParser.Create(site);
                var task = parser.Run();
                task.Wait();
            }
        }
    }
}
