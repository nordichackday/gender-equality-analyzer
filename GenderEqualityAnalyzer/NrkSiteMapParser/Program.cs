using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Repositories;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace NrkSiteMapParser
{
    class Program
    {
        static void Main(string[] args)
        {
            var task = Run();
            task.Wait();
        }

        static async Task Run()
        {
            var scraper = new NrkSiteMapScraper();

            var articleInfos = await scraper.Scrape();

            var queueHelper = new QueueHelper();
            var articleRepository = new ArticleRepository();

            foreach (var articleInfo in articleInfos)
            {
                if (!articleRepository.ContainsUrl(articleInfo.Url))
                {
                    Console.WriteLine($"Queueing {articleInfo.Url}");
                    queueHelper.Queue(articleInfo);
                }
                else
                {
                    Console.WriteLine($"{articleInfo.Url} was already in the database.");
                }
            }
        }
    }
}
