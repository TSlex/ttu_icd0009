using System;
using System.Collections.Generic;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DAL.App.EF.Helpers
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

        public static void SeedIdentity(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            var roleNames = new[]
            {
                new Role
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                    Name = "User"
                },
                new Role
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    Name = "Admin"
                },
                new Role
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000003"),
                    Name = "Student"
                },
                new Role
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000004"),
                    Name = "Teacher"
                }
            };

            foreach (var roleName in roleNames)
            {
                var role = roleManager.FindByNameAsync(roleName.Name).Result;
                if (role == null)
                {
                    role = new AppRole() {Id = roleName.Id, Name = roleName.Name};

                    var result = roleManager.CreateAsync(role).Result;

                    if (!result.Succeeded)
                    {
                        throw new ApplicationException("Role creation failed!");
                    }
                }
            }

            var users = new[]
            {
                new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                    UserName = "admin",
                    Email = "admin@admin.com",
                    Password = "Admin_123",
                    FirstName = "Administrator",
                    LastName = "",
                    RolesNames = new[]
                    {
                        "User", "Admin"
                    }
                },
                new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    UserName = "root",
                    Email = "root@root.com",
                    Password = "Admin_123",
                    FirstName = "Root",
                    LastName = "",
                    RolesNames = new[]
                    {
                        "User", "Admin"
                    }
                }
            };

            foreach (var user in users)
            {
                var newUser = userManager.FindByIdAsync(user.Id.ToString()).Result;
                if (newUser == null)
                {
                    newUser = new AppUser
                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserName = user.UserName,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        EmailConfirmed = true,
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
                else
                {
                    var resetToken = userManager.GeneratePasswordResetTokenAsync(newUser).Result;
                    var changePasswordResult =
                        userManager.ResetPasswordAsync(newUser, resetToken, user.Password).Result;
                    if (!changePasswordResult.Succeeded)
                    {
                        throw new ApplicationException("Passwords resetting failed!");
                    }
                }
            }
        }

        public static void SeedData(ApplicationDbContext ctx)
        {
            //Seed data equations here
            ctx.SaveChanges();
        }

        private struct User
        {
            public Guid Id { get; set; }

            public string FirstName { get; set; }
            public string LastName { get; set; }
            
            public string Email { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }

            public ICollection<string>? RolesNames { get; set; }
        }

        private struct Role
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
        }
    }
}