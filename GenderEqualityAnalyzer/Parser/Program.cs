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

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            GetSR();
        }

        static void GetSR()
        {
            var xml = GetSRXML().Result;

            XNamespace _xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";

            Console.WriteLine(xml.Descendants(XName.Get("url")));

            var urls = from x in xml.Elements(_xmlns + "urlset").Elements(_xmlns + "url")
                select x.Element(_xmlns + "loc").Value;


            var articles = Parse(urls);

            Console.WriteLine();

        }

        static IEnumerable<Article> Parse(IEnumerable<string> urls)
        {
            var tasks = new List<Task<Article>>();

            foreach (var url in urls)
            {
                var task = Parse(url);
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());

            return tasks.Select(x => x.Result);
        }

        static async Task<Article> Parse(string url)
        {
            var httpClient = new HttpClient();

            var html = await httpClient.GetStringAsync(url);

            var regex = new Regex(@"<meta property=\""og:image\"" content=\""([a-zA-Z0-9\/\:\.\-_]*)", RegexOptions.IgnoreCase);

            var match = regex.Match(html);
            if (match.Success)
            {
                var img = match.Groups[1].Value;
                Console.WriteLine(img);

                return new Article()
                {
                    ImageUrl = img
                };
            }
            return new Article();

        }

        private static async Task<XDocument> GetSRXML()
        {
            var httpClient = new HttpClient();

            return  XDocument.Parse(await httpClient.GetStringAsync("http://sverigesradio.se/sida/newssitemap.aspx"));

            
        }
    }

    
    


}
