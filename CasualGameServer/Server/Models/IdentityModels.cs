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
        public List<HighScore> HighScores { get; set; }
        public List<PlayerAchievement> UnlockedAchievemets { get; set; }

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
        [Required]
        public ApplicationUser User { get; set; }
        [Required]
        public int Score { get; set; }
    }


    public class Achievement
    {
        [Key]
        public int ID { get; set; }
        [Required, StringLength(16)]
        public string Name { get; set; }
        [Required, Display(Name="Name")]
        public string DisplayName { get; set; }
        [Required]
        public string ImageURL { get; set; }
        [Required]
        public string LockedDescription { get; set; }
        [Required]
        public string UnlockedDescription { get; set; }
    }


    public class PlayerAchievement
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public ApplicationUser Player { get; set; }
        [Required]
        public Achievement Achievement { get; set; }
    }
    

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        
        public DbSet<HighScore> Scores { get; set; }
        public DbSet<Achievement> Achievements { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        
    }

    public class SeedHighScores : DropCreateDatabaseAlways<ApplicationDbContext>
    {
        public override void InitializeDatabase(ApplicationDbContext context)
        {
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

            List<ApplicationUser> users = new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    Email = "noreply@nowhe.re",
                    UserName= "King_Kobra",
                    HighScores = new List<HighScore>()
                    {
                        new HighScore() { Score=1 },
                        new HighScore() {Score=5 }
                    }
                },
                new ApplicationUser()
                {
                    Email = "anyone@someone.com",
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
                    Email = "not@someone.com",
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
                    Email = "moop@tires.com",
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
                context.Users.Add(us);
                foreach (var sc in us.HighScores)
                {
                    context.Scores.Add(sc);
                }
            }

            base.InitializeDatabase(context);
        }
    }
}