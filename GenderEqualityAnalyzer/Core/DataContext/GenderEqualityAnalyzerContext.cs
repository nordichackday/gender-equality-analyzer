using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataContext
{
    public class GenderEqualityAnalyzerContext : DbContext
    {
        private static readonly string connStr = 
            "Server=tcp:genderequalityanalyzer.database.windows.net,1433;Database=GenderEqualityAnalyzerDb;User ID = sebbe@genderequalityanalyzer;Password=NordicHackday123;Encrypt=True;TrustServerCertificate=False;Connection Timeout = 30";
        public GenderEqualityAnalyzerContext() : base(connStr)
        { }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Face> Faces { get; set; }
        public DbSet<Site> Sites { get; set; }
    }
}
