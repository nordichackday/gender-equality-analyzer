using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DataContext;

namespace Core.Repositories
{
    public class SiteRepository : IDisposable
    {
        private GenderEqualityAnalyzerContext _dbContext;

        public SiteRepository()
        {
            _dbContext = new GenderEqualityAnalyzerContext();
        }

        public IEnumerable<Site> Get()
        {
            return _dbContext.Sites.ToList();
        } 
        



        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
