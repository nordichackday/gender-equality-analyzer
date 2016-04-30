using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NrkSiteMapParser
{
    public abstract class SitemapScraper
    {
        private string _siteMapUrl;
        private int _siteId;

        protected SitemapScraper(string siteMapUrl, int siteId)
        {
            this._siteMapUrl = siteMapUrl;
            this._siteId = siteId;
;        }

        public async Task<IEnumerable<ArticleInformation>> Scrape()
        {
            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XName xSitemap = xmlns + "sitemap";
            XName xLoc = xmlns + "loc";

            var siteMapXML = await GetXML(_siteMapUrl);

            var siteMapUrls = from x in siteMapXML.Descendants(xSitemap)
                select x.Element(xLoc).Value;


            var tasks = siteMapUrls.Select(ScrapeSitemap);
            Console.WriteLine($"Commencing scraping of {siteMapUrls.Count()} sitemaps");
            await Task.WhenAll(tasks);

            var result = tasks.SelectMany(x => x.Result);

            Console.WriteLine($"Finished scraping the sitemaps. Found {result.Count()} articles.");

            return result;
        }

        private async Task<IEnumerable<ArticleInformation>> ScrapeSitemap(string url)
        {
            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XName xUrl = xmlns + "url";
            XName xLoc = xmlns + "loc";
            XName xLastMod = xmlns + "lastmod";

            var siteMapXML = await GetXML(url);

            return from x in siteMapXML.Descendants(xUrl)
                select new ArticleInformation()
                {
                    SiteId = _siteId,
                    Url = x.Element(xLoc)?.Value,
                    LastModified = x.Element(xLastMod) != null ? GetDateOrNull(x.Element(xLastMod).Value) : null
                };
        }

        private DateTime? GetDateOrNull(string date)
        {
            DateTime retval;

            return DateTime.TryParse(date, out retval) ? retval : (DateTime?)null;

        }

        private async Task<XDocument> GetXML(string xmlUrl)
        {
            var httpClient = new HttpClient();

            return XDocument.Parse(await httpClient.GetStringAsync(xmlUrl));
        }
    }

    public class NrkSiteMapScraper : SitemapScraper
    {
        public NrkSiteMapScraper() : base("http://www.nrk.no/sitemap.xml", 3) { }
    }
}
