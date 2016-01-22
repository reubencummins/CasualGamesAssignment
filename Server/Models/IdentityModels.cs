using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class HighScore
    {
        [Key]
        public int ID { get; set; }

        public ApplicationUser User { get; set; }
        public int Score { get; set; }

    }

    public class ScoreTable
    {
        [Key]
        public int ID { get; set; }

        public string GameTitle { get; set; }
        
        public List<HighScore> Scores { get; set; }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ScoreTable HighScores { get; set; }
        public DbSet<HighScore> Scores { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Server.Models.ScoreTable> ScoreTables { get; set; }
    }

    public class SeedScoreTable : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        public override void InitializeDatabase(ApplicationDbContext context)
        {


            #region table
            ScoreTable table = new ScoreTable()
            {
                GameTitle = "Space Kings",
                Scores = new List<HighScore>()
                {
                    new HighScore()
                    {
                        User=new ApplicationUser()
                        {
                            UserName="FluffyKitten666"
                        },
                        Score=10000000
                    },
                    new HighScore()
                    {
                        User=new ApplicationUser()
                        {
                            UserName="FishermansFriend"
                        },
                        Score=10000000
                    },
                    new HighScore()
                    {
                        User=new ApplicationUser()
                        {
                            UserName="ElectricBoogaloo"
                        },
                        Score=10000000
                    },
                    new HighScore()
                    {
                        User=new ApplicationUser()
                        {
                            UserName="SaltyHalibut"
                        },
                        Score=1000000
                    },
                    new HighScore()
                    {
                        User=new ApplicationUser()
                        {
                            UserName="SuperMarxBros"
                        },
                        Score=100000
                    },
                    new HighScore()
                    {
                        User=new ApplicationUser()
                        {
                            UserName="Blutwurstest"
                        },
                        Score=10000
                    },
                    new HighScore()
                    {
                        User=new ApplicationUser()
                        {
                            UserName="CowhopBeboy"
                        },
                        Score=1000
                    },
                    new HighScore()
                    {
                        User=new ApplicationUser()
                        {
                            UserName="Callisto"
                        },
                        Score=100
                    },
                    new HighScore()
                    {
                        User=new ApplicationUser()
                        {
                            UserName="MeowMeowBeenz"
                        },
                        Score=10
                    },
                    new HighScore()
                    {
                        User=new ApplicationUser()
                        {
                            UserName="SpecialClothing"
                        },
                        Score=1
                    }
                }
            };
            #endregion

            foreach (var score in table.Scores)
            {
                context.Scores.Add(score);
                context.Users.Add(score.User);
            }
            context.HighScores = table;

            context.SaveChanges();
            base.InitializeDatabase(context);
        }
    }
}