namespace D2.Migrations
{
    using D2.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<D2.DAL.D2Context>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(D2.DAL.D2Context context)
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
            var heroes = new List<Hero>{
                new Hero {name = "Mirana", currentHealth = 100, maxHealth = 700, currentLV = 10},
                new Hero {name = "Lich", currentHealth = 100, maxHealth = 700, currentLV = 10},
                new Hero {name = "Dragon Knight", currentHealth = 100, maxHealth = 700, currentLV = 10}
            };
            heroes.ForEach(x => context.Heroes.AddOrUpdate(p => p.ID, x));
            context.SaveChanges();
        }
    }
}
