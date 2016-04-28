using System.Collections.Generic;
using Core.DataContext;

namespace Core.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Core.DataContext.GenderEqualityAnalyzerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Core.DataContext.GenderEqualityAnalyzerContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //


            context.Sites.AddOrUpdate(s => s.Name, new Site()
            {
                Articles = new List<Article>(),
                Name = "SverigesRadio",
                ParsingStrategy = ParsingStrategy.NewsSiteMap,
                Url = "http://sverigesradio.se/sida/newssitemap.aspx"
            });

        }
    }
}
