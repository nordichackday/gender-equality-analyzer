using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataContext;

namespace Core.Repositories
{
    public class ArticleRepository : IDisposable
    {
        private GenderEqualityAnalyzerContext _dbContext;

        public ArticleRepository()
        {
            _dbContext = new GenderEqualityAnalyzerContext();
        }

        public IEnumerable<Article> Get()
        {
            return _dbContext.Articles.ToList();
        } 

        public IEnumerable<Article> GetUnParsed()
        {
            return _dbContext.Articles.Where(x => x.IsImageParsed == false).ToList();
        }

        public Article Get(int id)
        {
            return _dbContext.Articles.Find(id);
        }

        public void Add(Article article, int siteId)
        {
            var site = _dbContext.Sites.Find(siteId);
            article.Site = site;
            _dbContext.Articles.Add(article);
        }

        public bool ContainsUrl(string url)
        {
            return _dbContext.Articles.Any(x => x.Url == url);
        }
        public void Update(Article article)
        {
            _dbContext.Entry(article).State = EntityState.Modified;
        }

        public IEnumerable<Article> FilterParsedArticles(IEnumerable<Article> articles, Site site)
        {
            var allArticles = _dbContext.Articles.Where(x => x.Site.Id == site.Id).ToList();

            return
                articles.Where(x => allArticles.All(a => a.Url != x.Url));
        }

        public void AddOrUpdate(IEnumerable<Article> articles)
        {
            _dbContext.Articles.AddOrUpdate(x => x.Url, articles.ToArray());

            _dbContext.SaveChanges();
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        public IEnumerable<Article> Get(Func<Article, bool> filter)
        {
            return _dbContext.Articles.Where(filter).ToList();
            
        }
    }
}
