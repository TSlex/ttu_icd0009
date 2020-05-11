using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace DAL.Helpers
{
    public class DataInitializers
    {
        public static void MigrateDatabase(ApplicationDbContext context)
        {
            context.Database.Migrate();
        }

        public static bool DeleteDatabase(ApplicationDbContext context)
        {
            return context.Database.EnsureDeleted();
        }

        public static async Task SeedIdentity(UserManager<Profile> userManager, RoleManager<MRole> roleManager)
        {
            var roleNames = new[] {new Role {Name = "User"}, new Role {Name = "Admin"}};

            foreach (var roleName in roleNames)
            {
                var role = roleManager.FindByNameAsync(roleName.Name).Result;
                if (role == null)
                {
                    role = new MRole();
                    role.Name = roleName.Name;

                    var result = roleManager.CreateAsync(role).Result;

                    if (!result.Succeeded)
                    {
                        throw new ApplicationException("Role creation failed!");
                    }
                }
            }

            var users = new[]
                {new User {Email = "admin@admin.com", Password = "Admin_123", RolesNames = new[] {"User", "Admin"}}};

            foreach (var user in users)
            {
                var newUser = await userManager.FindByEmailAsync(user.Email);
                if (newUser == null)
                {
                    newUser = new Profile
                    {
                        Email = user.Email,
                        UserName = user.Email,
                        EmailConfirmed = true
                    };

                    var result = userManager.CreateAsync(newUser, user.Password).Result;
                    if (!result.Succeeded)
                    {
                        throw new ApplicationException("User creation failed!");
                    }

                    foreach (var roleName in user.RolesNames!)
                    {
                        var roleResult = userManager.AddToRoleAsync(newUser, roleName).Result;

                        if (!roleResult.Succeeded)
                        {
                            throw new ApplicationException("User role assigment failed!");
                        }
                    }
                }
            }
        }

        public static void SeedData(ApplicationDbContext ctx)
        {
            //ChatRoles
            var chatRoles = new ChatRole[]
            {
                new ChatRole()
                {
                    RoleTitle = "Member",
                },
                new ChatRole()
                {
                    RoleTitle = "Creator",
                },
                new ChatRole()
                {
                    RoleTitle = "Moderator",
                },
                new ChatRole()
                {
                    RoleTitle = "Left",
                }
            };

            foreach (var chatRole in chatRoles)
            {
                if (!ctx.ChatRoles.Any(r => r.RoleTitle == chatRole.RoleTitle))
                {
                    ctx.ChatRoles.Add(chatRole);
                }
            }

            ctx.SaveChanges();

            //Ranks
            var ranks = new Rank[]
            {
                new Rank()
                {
                    RankTitle = "New User",
                    RankCode = "X_00",
                    RankDescription = "Welcome! We are happy to see new faces C:",
                    RankTextColor = "#000000",
                    RankColor = "#CCFFFF",
                    MinExperience = int.MinValue,
                    MaxExperience = 10
                },
                new Rank()
                {
                    RankTitle = "Newbie",
                    RankCode = "X_01",
                    RankDescription = "You are learning a new place",
                    RankTextColor = "#000000",
                    RankColor = "#99CCFF",
                    RankIcon = "star-half-alt;",
                    MinExperience = 11,
                    MaxExperience = 40
                },
                new Rank()
                {
                    RankTitle = "Amateur",
                    RankCode = "X_02",
                    RankDescription = "You feel a bit confident. What would wait you in future?",
                    RankTextColor = "#000000",
                    RankColor = "#3399FF",
                    RankIcon = "star;",
                    MinExperience = 41,
                    MaxExperience = 100
                },
                new Rank()
                {
                    RankTitle = "Apprentice",
                    RankCode = "X_03",
                    RankDescription =
                        "You have already learned the basics, but are you ready to move to the next level?",
                    RankTextColor = "#000000",
                    RankColor = "#0066FF",
                    RankIcon = "star;star-half-alt;",
                    MinExperience = 101,
                    MaxExperience = 500
                },
                new Rank()
                {
                    RankTitle = "Master",
                    RankCode = "X_04",
                    RankDescription =
                        "Yes, you feel confidence and even can teach the basics to Newbies. However, there is no limit to perfection C:",
                    RankTextColor = "#000000",
                    RankColor = "#6633FF",
                    RankIcon = "star;star;",
                    MinExperience = 501,
                    MaxExperience = 2000
                }
            };

            foreach (var rank in ranks)
            {
                if (!ctx.Ranks.Any(r => r.RankCode == rank.RankCode))
                {
                    ctx.Ranks.Add(rank);
                }
            }

            ctx.SaveChanges();

            var dbRanks = ctx.Ranks.AsNoTracking().Where(rank => rank.RankCode.Contains("X_")).ToList()
                .OrderBy(rank => rank.RankCode).ToArray();

            for (int index = 0; index < dbRanks.Length; index++)
            {
                if (index > 0)
                {
                    dbRanks[index].PreviousRankId = dbRanks[index - 1].Id;
                }

                if (index < dbRanks.Length - 1)
                {
                    dbRanks[index].NextRankId = dbRanks[index + 1].Id;
                }

                ctx.Ranks.Update(dbRanks[index]);
            }

            ctx.SaveChanges();

            //Gifts
            var gifts = new Gift[]
            {
                new Gift()
                {
                    GiftName = "New Horizon",
                    GiftCode = "X_00",
                    GiftImageUrl = "https://sun9-37.userapi.com/c855036/v855036822/224f58/Kuwvm_ds5yQ.jpg"
                },
                new Gift()
                {
                    GiftName = "1'st of May",
                    GiftCode = "X_01",
                    GiftImageUrl = "https://vk.com/images/gift/1086/256.jpg"
                },
                new Gift()
                {
                    GiftName = "Tasty",
                    GiftCode = "X_02",
                    GiftImageUrl = "https://vk.com/images/gift/1069/256.jpg"
                },
                new Gift()
                {
                    GiftName = "Bananas",
                    GiftCode = "X_03",
                    GiftImageUrl = "https://vk.com/images/gift/1063/256.jpg"
                },
                new Gift()
                {
                    GiftName = "LoveOfLove",
                    GiftCode = "X_04",
                    GiftImageUrl = "https://vk.com/images/gift/1005/256.jpg"
                }
            };

            foreach (var gift in gifts)
            {
                if (!ctx.Gifts.Any(g => g.GiftCode == gift.GiftCode))
                {
                    ctx.Gifts.Add(gift);
                }
            }

            ctx.SaveChanges();
        }
        
        struct User
        {
            public string Email { get; set; }
            public string Password { get; set; }

            public ICollection<string>? RolesNames { get; set; }
        }

        struct Role
        {
            public string Name { get; set; }
        }
    }
}