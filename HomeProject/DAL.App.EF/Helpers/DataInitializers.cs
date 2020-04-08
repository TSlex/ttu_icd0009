using System;
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

        public static void SeedIdentity(UserManager<Profile> userManager, RoleManager<MRole> roleManager)
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

            var users = new[] {new User{Login = "admin@admin.com", Password = "Admin_123", RolesNames = new []{"User", "Admin"}}};
            
            foreach (var user in users)
            {
                var newUser = userManager.FindByNameAsync(user.Login).Result;
                if (newUser == null)
                {
                    newUser = new Profile();
                    newUser.Email = user.Login;
                    newUser.UserName = user.Login;
                    
                    var result = userManager.CreateAsync(newUser, user.Password).Result;
                    if (!result.Succeeded)
                    {
                        throw new ApplicationException("User creation failed!");

                    }

                    foreach (var roleName in user.RolesNames)
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

        public static void SeedData(ApplicationDbContext context)
        {
        }
    }

    struct User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        
        public ICollection<string>? RolesNames { get; set; }
    }

    struct Role
    {
        public string Name { get; set; }
    }
}