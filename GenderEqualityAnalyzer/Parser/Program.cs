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
            GetSR();
        }

        static void GetSR()
        {
            var xml = GetSRXML().Result;

            XNamespace _xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XNamespace _xmlnsn = "http://www.google.com/schemas/sitemap-news/0.9";
            Console.WriteLine(xml.Descendants(XName.Get("url")));
            
            var articles = from x in xml.Elements(_xmlns + "urlset").Elements(_xmlns + "url")
                select new Article()
                {
                    Url = x.Element(_xmlns + "loc").Value,
                    CreatedDate = DateTime.Now,
                    PublishedDate =
                        GetDateTimeOrDefault(x.Element(_xmlnsn + "news").Element(_xmlnsn + "publication_date").Value),
                    Title = x.Element(_xmlnsn + "news").Element(_xmlnsn + "title").Value
                };

            var articleRepository = new ArticleRepository();

            articles = Parse(articleRepository.FilterParsedArticles(articles));

            articleRepository.AddOrUpdate(articles);
        }

        private static DateTime GetDateTimeOrDefault(string date)
        {
            DateTime retval;
            return DateTime.TryParse(date, out retval) ? retval : DateTime.MinValue;
        }
        static IEnumerable<Article> Parse(IEnumerable<Article> articles)
        {
            var tasks = new List<Task<Article>>();

            foreach (var article in articles)
            {
                var task = Parse(article);
                tasks.Add(task);
            }

            Task.WaitAll(tasks.ToArray());
            return tasks.Select(x => x.Result);
        }

        static async Task<Article> Parse(Article article)
        {
            var httpClient = new HttpClient();

            var html = await httpClient.GetStringAsync(article.Url);

            var regex = new Regex(@"<meta property=\""og:image\"" content=\""([a-zA-Z0-9\/\:\.\-_]*)", RegexOptions.IgnoreCase);

            var match = regex.Match(html);
            if (match.Success)
                article.ImageUrl = match.Groups[1].Value;

            return article;
        }

        private static async Task<XDocument> GetSRXML()
        {
            var httpClient = new HttpClient();

            return  XDocument.Parse(await httpClient.GetStringAsync("http://sverigesradio.se/sida/newssitemap.aspx"));
            
        }
    }
}
