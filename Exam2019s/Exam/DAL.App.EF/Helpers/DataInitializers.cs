using System;
using System.Collections.Generic;
using System.Linq;
using Domain;
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
                },
                //testing
                new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000003"),
                    UserName = "kukala",
                    Email = "kukala@a.com",
                    Password = "Admin_123",
                    FirstName = "Kustav",
                    LastName = "Kala",
                    RolesNames = new[]
                    {
                        "User", "Student"
                    }
                },
                new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000004"),
                    UserName = "akaver",
                    Email = "akaver@a.com",
                    Password = "Admin_123",
                    FirstName = "Andres",
                    LastName = "Kaver",
                    RolesNames = new[]
                    {
                        "User", "Teacher"
                    }
                },
                new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000009"),
                    UserName = "mkalmo",
                    Email = "akaver@a.com",
                    Password = "Admin_123",
                    FirstName = "Mart",
                    LastName = "Kalmo",
                    RolesNames = new[]
                    {
                        "User", "Teacher"
                    }
                },
                new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000005"),
                    UserName = "aleksi",
                    Email = "aleksi@a.com",
                    Password = "Admin_123",
                    FirstName = "Aleksandr",
                    LastName = "Ivanov",
                    RolesNames = new[]
                    {
                        "User", "Student"
                    }
                },
                new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000006"),
                    UserName = "alkeze",
                    Email = "alkeze@a.com",
                    Password = "Admin_123",
                    FirstName = "Aleksandr",
                    LastName = "Kezerev",
                    RolesNames = new[]
                    {
                        "User", "Student"
                    }
                },
                new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000007"),
                    UserName = "vkrug",
                    Email = "vkrug@a.com",
                    Password = "Admin_123",
                    FirstName = "Vladislav",
                    LastName = "Kruglov",
                    RolesNames = new[]
                    {
                        "User"
                    }
                },
                new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000008"),
                    UserName = "wareware",
                    Email = "wareware@a.com",
                    Password = "Admin_123",
                    FirstName = "Valentina",
                    LastName = "Selivanova",
                    RolesNames = new[]
                    {
                        "User"
                    }
                },
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
            //Semesters
            var semesters = new Semester[]
            {
                new Semester()
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                    Title = "FALL",
                    Code = "fall",
                },
                new Semester()
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    Title = "SPRING",
                    Code = "spring",
                },
            };

            foreach (var semester in semesters)
            {
                if (!ctx.Semesters.Any(e => e.Id == semester.Id))
                {
                    ctx.Semesters.Add(semester);
                }
            }

            ctx.SaveChanges();
            
            //Gifts
            var subjects = new Subject[]
            {
                new Subject()
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                    SemesterId = new Guid("00000000-0000-0000-0000-000000000001"),
                    TeacherId = new Guid("00000000-0000-0000-0000-000000000009"),
                    SubjectTitle = "Java",
                    SubjectCode = "ICD0019",
                    HomeWorks = new List<HomeWork>()
                    {
                        new HomeWork()
                        {
                            Description = "TEST1",
                            Title = "Hello1",
                        },
                        new HomeWork()
                        {
                            Description = "TEST2",
                            Title = "Hello2",
                        },
                        new HomeWork()
                        {
                            Description = "TEST3",
                            Title = "Hello3",
                        },
                    }
                },
                new Subject()
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    SemesterId = new Guid("00000000-0000-0000-0000-000000000001"),
                    TeacherId = new Guid("00000000-0000-0000-0000-000000000009"),
                    SubjectTitle = "Veebitehnoloogiad",
                    SubjectCode = "ICD0007",
                    HomeWorks = new List<HomeWork>()
                    {
                        new HomeWork()
                        {
                            Description = "TEST1",
                            Title = "Hello1",
                        },
                        new HomeWork()
                        {
                            Description = "TEST2",
                            Title = "Hello2",
                        },
                        new HomeWork()
                        {
                            Description = "TEST3",
                            Title = "Hello3",
                        },
                    }
                },
                new Subject()
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000003"),
                    SemesterId = new Guid("00000000-0000-0000-0000-000000000002"),
                    TeacherId = new Guid("00000000-0000-0000-0000-000000000009"),
                    SubjectTitle = "Veebirakendused Java baasil",
                    SubjectCode = "ICD0011",
                    HomeWorks = new List<HomeWork>()
                    {
                        new HomeWork()
                        {
                            Description = "TEST1",
                            Title = "Hello1",
                        },
                        new HomeWork()
                        {
                            Description = "TEST2",
                            Title = "Hello2",
                        },
                        new HomeWork()
                        {
                            Description = "TEST3",
                            Title = "Hello3",
                        },
                        new HomeWork()
                        {
                            Description = "TEST4",
                            Title = "Hello4",
                        },
                    }
                },
                new Subject()
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000004"),
                    SemesterId = new Guid("00000000-0000-0000-0000-000000000001"),
                    TeacherId = new Guid("00000000-0000-0000-0000-000000000004"),
                    SubjectTitle = "Hajussüsteemide ehitamine",
                    SubjectCode = "ICD0009",
                    HomeWorks = new List<HomeWork>()
                    {
                        new HomeWork()
                        {
                            Description = "TEST1",
                            Title = "Hello1",
                        },
                        new HomeWork()
                        {
                            Description = "TEST2",
                            Title = "Hello2",
                        },
                        new HomeWork()
                        {
                            Description = "TEST3",
                            Title = "Hello3",
                        },
                    }
                },
                new Subject()
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000005"),
                    SemesterId = new Guid("00000000-0000-0000-0000-000000000002"),
                    TeacherId = new Guid("00000000-0000-0000-0000-000000000004"),
                    SubjectTitle = "ASP.NET Veebirakendused",
                    SubjectCode = "ICD0015",
                    HomeWorks = new List<HomeWork>()
                    {
                        new HomeWork()
                        {
                            Description = "TEST1",
                            Title = "Hello1",
                        },
                        new HomeWork()
                        {
                            Description = "TEST2",
                            Title = "Hello2",
                        },
                    }
                },
                new Subject()
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000006"),
                    SemesterId = new Guid("00000000-0000-0000-0000-000000000002"),
                    TeacherId = new Guid("00000000-0000-0000-0000-000000000004"),
                    SubjectTitle = "JavaScript",
                    SubjectCode = "ICD0006",
                    HomeWorks = new List<HomeWork>()
                    {
                        new HomeWork()
                        {
                            Description = "TEST1",
                            Title = "Hello1",
                        },
                        new HomeWork()
                        {
                            Description = "TEST2",
                            Title = "Hello2",
                        },
                        new HomeWork()
                        {
                            Description = "TEST3",
                            Title = "Hello3",
                        },
                    }
                },
            };

            foreach (var subject in subjects)
            {
                if (!ctx.Subjects.Any(e => e.Id == subject.Id))
                {
                    ctx.Subjects.Add(subject);
                }
            }

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