using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Core.DataContext;
using Core.Results;

namespace Core.Repositories
{
    public class FaceRepository
    {
        private GenderEqualityAnalyzerContext _dbContext;
        public FaceRepository()
        {
            _dbContext = new GenderEqualityAnalyzerContext();
        }

        public object GetForStartPage(string siteName)
        {
            var faces = _dbContext.Faces.Where(x => x.Article.Site.Name == siteName).ToList();

            var women = faces.Count(x => x.Gender == Gender.Female);
            var men = faces.Count(x => x.Gender == Gender.Male);
            var total = faces.Count;

            return new FaceStartPageResult()
            {
                WomenPercentage = (int) (Math.Round((double)women/total, 2) * 100),
                MenPercentage = (int) (Math.Round((double)men/total,2) * 100),
                Broadcaster = siteName
            };

        }
    }
    
}
