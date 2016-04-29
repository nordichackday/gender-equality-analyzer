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

        public object GetForDetailsPage(string siteName)
        {
            var faces = _dbContext.Faces.Where(x => x.Article.Site.Name == siteName).ToList();

            var women = faces.Where(x => x.Gender == Gender.Female);
            var averageAgeWoman = women.Average(x => x.Age);
            var happyFactorWoman = ((double) women.Count(x => x.SmileFactor > 0.6) / women.Count()) * 100;
            var sadFactorWoman = ((double)women.Count(x => x.SmileFactor < 0.4) / women.Count()) * 100;
            var mustachFactorWoman = women.Average(x => x.MoustacheFactor);
            var beardFactorWoman = women.Average(x => x.BeardFactor);
            var sideburnFactorWoman = women.Average(x => x.SideburnsFactor);


            var men = faces.Where(x => x.Gender == Gender.Male);
            var averageAgeMen = men.Average(x => x.Age);
            var happyFactorMen = ((double)men.Count(x => x.SmileFactor > 0.6) / men.Count()) * 100;
            var sadFactoMen = ((double)men.Count(x => x.SmileFactor < 0.4) / men.Count()) * 100;
            var mustachFactorMen = men.Average(x => x.MoustacheFactor) * 100;
            var beardFactorMen = men.Average(x => x.BeardFactor) * 100;
            var sideburnFactorMen = men.Average(x => x.SideburnsFactor) * 100;



            return new DetailsPageResult
            {
                Broadcaster = siteName,
                AverageAgeWoman = Math.Round(averageAgeWoman,1),
                HappyFactorWoman = Math.Round(happyFactorWoman),
                SadFactorWoman = Math.Round(sadFactorWoman),
                MustachFactorWoman = Math.Round(mustachFactorWoman),
                BeardFactorWoman = Math.Round(beardFactorWoman),
                SideburnFactorWoman = Math.Round(sideburnFactorWoman),
                AverageAgeMen = Math.Round(averageAgeMen,1),
                HappyFactorMen = Math.Round(happyFactorMen),
                SadFactorMen = Math.Round(sadFactoMen),
                MustachFactorMen = Math.Round(mustachFactorMen),
                BeardFactorMen = Math.Round(beardFactorMen),
                SideburnFactorMen = Math.Round(sideburnFactorMen)
            };

        }
    }
    
}
