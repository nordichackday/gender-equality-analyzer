using System;
using System.Collections.Generic;
using System.Linq;
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
                WomenPercentage = (int) (Math.Round((double) women/total, 2)*100),
                MenPercentage = (int) (Math.Round((double) men/total, 2)*100),
                Broadcaster = siteName
            };
        }

        public ChartsPageResult GetForChartsPage()
        {
            var result = new ChartsPageResult();
            result.Broadcasters = new List<ChartsPageBroadcasterResult>();
            foreach (var site in _dbContext.Sites)
            {
                var faces = _dbContext.Faces.Where(x => x.Article.Site.Id == site.Id);

                result.Broadcasters.Add(new ChartsPageBroadcasterResult(Round(faces.Average(x => x.Age)),
                    Round(((double) faces.Count(x => x.SmileFactor > 0.6)/faces.Count())*100),
                    Round(((double) faces.Count(x => x.SmileFactor < 0.4)/faces.Count())*100),
                    Round(faces.Average(x => x.MoustacheFactor) * 100),
                    Round(faces.Average(x => x.BeardFactor) * 100),
                    Round(faces.Average(x => x.SideburnsFactor) * 100),
                    site.Name
                    ));
            }

            return result;
        }

        private double Round(double val)
        {
            return Math.Round(val, 0);
        }


        public object GetForDetailsPage(string siteName)
        {
            var faces = _dbContext.Faces.Where(x => x.Article.Site.Name == siteName).ToList();

            var women = faces.Where(x => x.Gender == Gender.Female);
            var averageAgeWoman = women.Average(x => x.Age);
            var happyFactorWoman = ((double) women.Count(x => x.SmileFactor > 0.6)/women.Count())*100;
            var sadFactorWoman = ((double) women.Count(x => x.SmileFactor < 0.4)/women.Count())*100;
            var mustachFactorWoman = women.Average(x => x.MoustacheFactor);
            var beardFactorWoman = women.Average(x => x.BeardFactor);
            var sideburnFactorWoman = women.Average(x => x.SideburnsFactor);


            var men = faces.Where(x => x.Gender == Gender.Male);
            var averageAgeMen = men.Average(x => x.Age);
            var happyFactorMen = ((double) men.Count(x => x.SmileFactor > 0.6)/men.Count())*100;
            var sadFactoMen = ((double) men.Count(x => x.SmileFactor < 0.4)/men.Count())*100;
            var mustachFactorMen = men.Average(x => x.MoustacheFactor)*100;
            var beardFactorMen = men.Average(x => x.BeardFactor)*100;
            var sideburnFactorMen = men.Average(x => x.SideburnsFactor)*100;


            return new DetailsPageResult
            {
                Broadcaster = siteName,
                AverageAgeWoman = Math.Round(averageAgeWoman, 1),
                HappyFactorWoman = Math.Round(happyFactorWoman),
                SadFactorWoman = Math.Round(sadFactorWoman),
                MustachFactorWoman = Math.Round(mustachFactorWoman),
                BeardFactorWoman = Math.Round(beardFactorWoman),
                SideburnFactorWoman = Math.Round(sideburnFactorWoman),
                AverageAgeMen = Math.Round(averageAgeMen, 1),
                HappyFactorMen = Math.Round(happyFactorMen),
                SadFactorMen = Math.Round(sadFactoMen),
                MustachFactorMen = Math.Round(mustachFactorMen),
                BeardFactorMen = Math.Round(beardFactorMen),
                SideburnFactorMen = Math.Round(sideburnFactorMen)
            };
        }
    }

    public class ChartsPageResult
    {
        public List<ChartsPageBroadcasterResult> Broadcasters { get; set; }
    }

    public class ChartsPageBroadcasterResult
    {
        public double AverageAge { get; set; }
        public double AverageHappiness { get; set; }
        public double AverageSadness { get; set; }
        public double AverageMustasch { get; set; }
        public double AverageBeard { get; set; }
        public double AverageSideburn { get; set; }
        public string Broadcaster { get; set; }
        public ChartsPageBroadcasterResult(double averageAge,
            double averageHappiness,
            double averageSadness,
            double averageMustasch,
            double averageBeard,
            double averageSideburn, string broadcaster)
        {
            AverageAge = averageAge;
            AverageHappiness = averageHappiness;
            AverageSadness = averageSadness;
            AverageMustasch = averageMustasch;
            AverageBeard = averageBeard;
            AverageSideburn = averageSideburn;
            Broadcaster = broadcaster;
        }
    }
}