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

        public Article Get(int id)
        {
            return _dbContext.Articles.Find(id);
        }

        public void Add(Article article)
        {
            _dbContext.Articles.Add(article);
        }

        public void Update(Article article)
        {
            _dbContext.Entry(article).State = EntityState.Modified;
        }

        public IEnumerable<Article> FilterParsedArticles(IEnumerable<Article> articles)
        {
            return
                articles.Where(x => !_dbContext.Articles.Any(a => a.Url == x.Url || x.PublishedDate > a.PublishedDate));
        }

        public void AddOrUpdate(IEnumerable<Article> articles)
        {
            _dbContext.Articles.AddOrUpdate(x => x.Url, articles.ToArray());

            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
