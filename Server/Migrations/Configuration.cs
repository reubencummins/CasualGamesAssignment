namespace Server.Migrations
{
    using Microsoft.AspNet.Identity;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Server.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }
        int i = -1;
        private int NextEmail()
        {
            i++;
            return i;
        }

        protected override void Seed(Server.Models.ApplicationDbContext context)
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

            List<Achievement> achievements = new List<Achievement>()
            {
                new Achievement()
                {
                    Name="HighScore",
                    DisplayName="Skywalker",
                    LockedDescription="Score very high",
                    UnlockedDescription ="You scored over 10000",
                    ImageURL="\\Content\\Images\\skywalker.jpg"
                },
                new Achievement()
                {
                    Name="Survive",
                    DisplayName="Ripley",
                    LockedDescription="Survive against the odds",
                    UnlockedDescription ="You survived for over ten minutes",
                    ImageURL="\\Content\\Images\\ripley.jpg"
                },
                new Achievement()
                {
                    Name="TripleKill",
                    DisplayName="Deckard",
                    LockedDescription="\"Retire\" three enemies within a second.",
                    UnlockedDescription ="You \"retired\" three enemies within a second.",
                    ImageURL="\\Content\\Images\\deckard.jpg"
                },
                new Achievement()
                {
                    Name="SecretItem",
                    DisplayName="Muad'dib",
                    LockedDescription="A secret...",
                    UnlockedDescription ="You found the sacred relic.",
                    ImageURL="\\Content\\Images\\muad'dib.jpg"
                }
            };

            foreach (var ach in achievements)
            {
                context.Achievements.Add(ach);
            }

            PasswordHasher hasher = new PasswordHasher();
            List<ApplicationUser> users = new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    Email = NextEmail() + "@web.com",
                    UserName= "King_Kobra",
                    HighScores = new List<HighScore>()
                    {
                        new HighScore() { Score=1 },
                        new HighScore() {Score=5 }
                    }
                },
                new ApplicationUser()
                {
                    Email = NextEmail() + "@web.com",
                    UserName= "WaltzingMatilda",
                    HighScores = new List<HighScore>()
                    {
                        new HighScore() { Score = 120 },
                        new HighScore() { Score = 54 },
                        new HighScore() { Score= 555 }
                    }
                },
                new ApplicationUser()
                {
                    Email = NextEmail() + "@web.com",
                    UserName= "IsildursBane",
                    HighScores = new List<HighScore>()
                    {
                        new HighScore() { Score = 333 },
                        new HighScore() { Score = 5654 },
                        new HighScore() { Score= 12 }
                    }
                },
                new ApplicationUser()
                {
                    Email = NextEmail() + "@web.com",
                    UserName= "MoopBrigade",
                    HighScores = new List<HighScore>()
                    {
                        new HighScore() { Score = 1251 },
                        new HighScore() { Score = 784 },
                        new HighScore() { Score= 1243 },
                        new HighScore() { Score = 565 }
                    }
                },
            };
            foreach (var us in users)
            {
                us.PasswordHash = hasher.HashPassword("password");
                context.Users.Add(us);
                foreach (var sc in us.HighScores)
                {
                    context.Scores.Add(sc);
                }
            }
            context.SaveChanges();
        }
    }
}
