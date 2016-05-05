using System;
using System.Data.Entity;

namespace Core.DataContext
{
    public class GenderEqualityAnalyzerContext : DbContext
    {
        private static readonly string connStr =
            "Server=tcp:genderequalityanalyzer.database.windows.net,1433;Database=GenderEqualityAnalyzerDb;User ID = sebbe@genderequalityanalyzer;Password=NordicHackday123;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;MultipleActiveResultSets=true;";

        public GenderEqualityAnalyzerContext() : base(connStr)
        {
            var type = typeof (System.Data.Entity.SqlServer.SqlProviderServices);
            if (type == null)
                throw new Exception("Do not remove, ensures static reference to System.Data.Entity.SqlServer");
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Face> Faces { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<BlacklistedImageUrl> BlacklistedImageUrls { get; set; }
    }
}