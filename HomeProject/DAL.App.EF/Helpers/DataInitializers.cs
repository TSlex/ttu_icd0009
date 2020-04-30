using System;
using System.Linq;
using System.Collections.Generic;
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

        public static async void SeedIdentity(UserManager<Profile> userManager, RoleManager<MRole> roleManager)
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

            var users = new[] {new User{Email = "admin@admin.com", Password = "Admin_123", RolesNames = new []{"User", "Admin"}}};
            
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

        public static async void SeedData(ApplicationDbContext ctx)
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

            await ctx.SaveChangesAsync();
            
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
                    MaxExperience = 10
                },
                new Rank()
                {
                    RankTitle = "Newbie",
                    RankCode = "X_01",
                    RankDescription = "You are learning a new place",
                    RankTextColor = "#000000",
                    RankColor = "#33FFCC",
                    RankIcon = "star-half-alt;",
                    MaxExperience = 40
                },
                new Rank()
                {
                    RankTitle = "Apprentice",
                    RankCode = "X_02",
                    RankDescription = "You feel a bit confident. What would wait you in future?",
                    RankTextColor = "#000000",
                    RankColor = "#00CCFF",
                    RankIcon = "star;star;",
                    MaxExperience = 100
                }
            };
            
            foreach (var rank in ranks)
            {
                if (!ctx.Ranks.Any(r => r.RankCode == rank.RankCode))
                {
                    ctx.Ranks.Add(rank);
                }
            }

            await ctx.SaveChangesAsync();
            
            //Gifts
            var gifts = new Gift[]
            {
                new Gift()
                {
                    GiftName = "New Horizon",
                    GiftCode = "X_00",
                    GiftImageUrl = "https://sun9-37.userapi.com/c855036/v855036822/224f58/Kuwvm_ds5yQ.jpg"
                }
            };

            foreach (var gift in gifts)
            {
                if (!ctx.Gifts.Any(g => g.GiftCode == gift.GiftCode))
                {
                    ctx.Gifts.Add(gift);
                }
            }

            await ctx.SaveChangesAsync();
        }
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