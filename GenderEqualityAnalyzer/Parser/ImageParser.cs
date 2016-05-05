using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using Core.DataContext;
using Core.Repositories;

namespace Parser
{
    public abstract class ImageParser
    {
        private ArticleRepository _articleRepository;
        protected Site _site;
        protected virtual Regex ImageRegex => new Regex(@"<meta property=\""og:image\"" content=\""([\S]*)", RegexOptions.IgnoreCase);

        protected ImageParser(Site site)
        {
            _articleRepository = new ArticleRepository();
            _site = site;
        }

        public static ImageParser Create(Site site)
        {
            switch (site.ParsingStrategy)
            {
                case ParsingStrategy.NewsSiteMap:
                    return new NewsSiteMapImageParser(site);
                case ParsingStrategy.SiteMap:
                    return new SiteMapImageParser(site);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected async Task Parse(IEnumerable<Article> articles)
        {
            var counter = 0;
            var count = articles.Count();

            articles = _articleRepository.FilterParsedArticles(articles, _site);

            foreach (var article in articles)
            {
                try
                {
                    var art = await Parse(article);
                    _articleRepository.Add(art, _site.Id);
                    await _articleRepository.Save();
                }
                catch { }

                Console.WriteLine($"{counter++} av {count}");
            }
            
            //var tasks = _articleRepository.FilterParsedArticles(articles).Select(Parse).ToArray();

            //return await Task.WhenAll(tasks); 
        }

        private async Task<Article> Parse(Article article)
        {
            if (article.Url == null)
                return article;
            Console.WriteLine(article.Url);
            var httpClient = new HttpClient();

            var html = await httpClient.GetStringAsync(article.Url);
            if (html == null)
                return article;
            var match = ImageRegex.Match(html);
            if (match.Success)
                article.ImageUrl = match.Groups[1].Value;
            
            return article;
        }

        protected async Task<XDocument> GetXML(string xmlUrl)
        {
            var httpClient = new HttpClient();

            return XDocument.Parse(await httpClient.GetStringAsync(xmlUrl));
        }


        public async Task Run()
        {
            await Parse(await GetArticlesAsync());

            //_articleRepository.AddOrUpdate(articles);
        }

        protected virtual async Task<IEnumerable<Article>> GetArticlesAsync()
        {
            return null;
        }


        protected static DateTime GetDateTimeOrDefault(string date)
        {
            DateTime retval;
            return DateTime.TryParse(date, out retval) ? retval : DateTime.MinValue;
        }
    }

    public class SiteMapImageParser : ImageParser
    {
        public SiteMapImageParser(Site site) : base(site) { }

        protected override async Task<IEnumerable<Article>> GetArticlesAsync()
        {
            var xml = await GetXML(_site.Url);

            XNamespace _xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";

            var siteMaps = from x in xml.Descendants(_xmlns + "sitemap")
                select x.Element(_xmlns + "loc").Value;


            var tasks = siteMaps.Take(30).Select(ParseSiteMap);

            await Task.WhenAll(tasks);

            return tasks.SelectMany(x => x.Result);
        }

        private async Task<IEnumerable<Article>> ParseSiteMap(string url)
        {
            var xml = await GetXML(url);

            XNamespace _xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";

            var articles = from x in xml.Descendants(_xmlns + "url")
                select new Article()
                {
                    CreatedDate = DateTime.Now,
                    Url = x.Element(_xmlns + "loc").Value,
                    PublishedDate = GetDateTimeOrDefault(x.Element(_xmlns + "lastmod") != null ? x.Element(_xmlns + "lastmod").Value : "2015-01-01"),
                };

            return articles;

        }
    }

    public class NewsSiteMapImageParser : ImageParser
    {
        public NewsSiteMapImageParser(Site site) : base(site){ }

        protected override async Task<IEnumerable<Article>>  GetArticlesAsync()
        {
            var xml = await GetXML(_site.Url);

            XNamespace _xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XNamespace _xmlnsn = "http://www.google.com/schemas/sitemap-news/0.9";

            var articles = from x in xml.Elements(_xmlns + "urlset").Elements(_xmlns + "url")
                           select new Article()
                           {
                               Url = x.Element(_xmlns + "loc").Value,
                               CreatedDate = DateTime.Now,
                               PublishedDate =
                                   GetDateTimeOrDefault(x.Element(_xmlnsn + "news").Element(_xmlnsn + "publication_date").Value),
                               Title = x.Element(_xmlnsn + "news").Element(_xmlnsn + "title").Value
                           };

            return articles;
        }


    }
}
