using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Domain.Enums;
using Domain.Identity;
using Domain.Translation;
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
                {new User {UserName = "admin", Email = "admin@admin.com", Password = "Admin_123", RolesNames = new[] {"User", "Admin"}}};

            foreach (var user in users)
            {
                var newUser = await userManager.FindByEmailAsync(user.Email);
                if (newUser == null)
                {
                    newUser = new Profile
                    {
                        Id = new Guid("00000000-0000-0000-0000-000000000001"),
                        Email = user.Email,
                        UserName = user.UserName,
                        EmailConfirmed = true,
                        ProfileRanks = new List<ProfileRank>()
                        {
                            new ProfileRank()
                            {
                                ProfileId = new Guid("00000000-0000-0000-0000-000000000001"),
                                RankId = new Guid("00000000-0000-0000-0000-000000000001"),
                            }
                        }
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
                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                    RoleTitle = "Member",
                    RoleTitleValue = new LangString()
                    {
                        Translations = new List<Translation>()
                        {
                            new Translation()
                            {
                                Culture = "en",
                                Value = "Member"
                            },
                            new Translation()
                            {
                                Culture = "ru",
                                Value = "Участник"
                            },
                            new Translation()
                            {
                                Culture = "et",
                                Value = "Osaleja"
                            }
                        }
                    },
                    CanRenameRoom = true,
                    CanEditMembers = false,
                    CanEditMessages = true,
                    CanEditAllMessages = false,
                    CanWriteMessages = true,
                },
                new ChatRole()
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    RoleTitle = "Creator",
                    RoleTitleValue = new LangString()
                    {
                        Translations = new List<Translation>()
                        {
                            new Translation()
                            {
                                Culture = "en",
                                Value = "Creator"
                            },
                            new Translation()
                            {
                                Culture = "ru",
                                Value = "Создатель"
                            },
                            new Translation()
                            {
                                Culture = "et",
                                Value = "Looja"
                            }
                        }
                    },

                    CanRenameRoom = true,
                    CanEditMembers = true,
                    CanEditMessages = true,
                    CanEditAllMessages = true,
                    CanWriteMessages = true,
                },
                new ChatRole()
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000003"),
                    RoleTitle = "Moderator",
                    RoleTitleValue = new LangString()
                    {
                        Translations = new List<Translation>()
                        {
                            new Translation()
                            {
                                Culture = "en",
                                Value = "Moderator"
                            },
                            new Translation()
                            {
                                Culture = "ru",
                                Value = "Модератор"
                            },
                            new Translation()
                            {
                                Culture = "et",
                                Value = "Moderaator"
                            }
                        }
                    },

                    CanRenameRoom = true,
                    CanEditMembers = true,
                    CanEditMessages = true,
                    CanEditAllMessages = true,
                    CanWriteMessages = true,
                },
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
                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                    RankTitle = new LangString()
                    {
                        Translations = new List<Translation>()
                        {
                            new Translation()
                            {
                                Culture = "en",
                                Value = "New User",
                            },
                            new Translation()
                            {
                                Culture = "et",
                                Value = "Uus kasutaja",
                            },
                            new Translation()
                            {
                                Culture = "ru",
                                Value = "Новый пользователь",
                            }
                        }
                    },
                    RankCode = "X_00",
                    RankDescription = new LangString()
                    {
                        Translations = new List<Translation>()
                        {
                            new Translation()
                            {
                                Culture = "en",
                                Value = "Welcome! We are happy to see new faces C:",
                            },
                            new Translation()
                            {
                                Culture = "et",
                                Value = "Tere tulemast! Meil on hea meel näha uusi nägusid C:",
                            },
                            new Translation()
                            {
                                Culture = "ru",
                                Value = "Добро пожаловать! Мы рады всегда рады новым лицам С:",
                            }
                        }
                    },
                    RankTextColor = "#000000",
                    RankColor = "#CCFFFF",
                    MinExperience = int.MinValue,
                    MaxExperience = 10
                },
                new Rank()
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    RankTitle = new LangString()
                    {
                        Translations = new List<Translation>()
                        {
                            new Translation()
                            {
                                Culture = "en",
                                Value = "Newbie",
                            },
                            new Translation()
                            {
                                Culture = "et",
                                Value = "Algaja",
                            },
                            new Translation()
                            {
                                Culture = "ru",
                                Value = "Новичек",
                            }
                        }
                    },
                    RankCode = "X_01",
                    RankDescription = new LangString()
                    {
                        Translations = new List<Translation>()
                        {
                            new Translation()
                            {
                                Culture = "en",
                                Value = "You are learning a new place",
                            },
                            new Translation()
                            {
                                Culture = "et",
                                Value = "Sa oled leidnud uut koht",
                            },
                            new Translation()
                            {
                                Culture = "ru",
                                Value = "Ты изучаешь новое место",
                            }
                        }
                    },
                    RankTextColor = "#000000",
                    RankColor = "#99CCFF",
                    RankIcon = "star-half-alt;",
                    MinExperience = 10,
                    MaxExperience = 40
                },
                new Rank()
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000003"),
                    RankTitle = new LangString()
                    {
                        Translations = new List<Translation>()
                        {
                            new Translation()
                            {
                                Culture = "en",
                                Value = "Amateur",
                            },
                            new Translation()
                            {
                                Culture = "et",
                                Value = "Amatöör",
                            },
                            new Translation()
                            {
                                Culture = "ru",
                                Value = "Любитель",
                            }
                        }
                    },
                    RankCode = "X_02",
                    RankDescription = new LangString()
                    {
                        Translations = new List<Translation>()
                        {
                            new Translation()
                            {
                                Culture = "en",
                                Value = "You feel a bit confident. What would wait you in future?",
                            },
                            new Translation()
                            {
                                Culture = "et",
                                Value = "Tunned end natuke enesekindlalt. Mis teid tulevikus ootaks?",
                            },
                            new Translation()
                            {
                                Culture = "ru",
                                Value = "Вы чувствуете себя немного уверенно. Что будет ждать вас в будущем?",
                            }
                        }
                    },
                    RankTextColor = "#000000",
                    RankColor = "#3399FF",
                    RankIcon = "star;",
                    MinExperience = 40,
                    MaxExperience = 100
                },
                new Rank()
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000004"),
                    RankTitle = new LangString()
                    {
                        Translations = new List<Translation>()
                        {
                            new Translation()
                            {
                                Culture = "en",
                                Value = "Apprentice",
                            },
                            new Translation()
                            {
                                Culture = "et",
                                Value = "Õpipoiss",
                            },
                            new Translation()
                            {
                                Culture = "ru",
                                Value = "Подмастерье",
                            }
                        }
                    },
                    RankCode = "X_03",
                    RankDescription = new LangString()
                    {
                        Translations = new List<Translation>()
                        {
                            new Translation()
                            {
                                Culture = "en",
                                Value =
                                    "You have already learned the basics, but are you ready to move to the next level?",
                            },
                            new Translation()
                            {
                                Culture = "et",
                                Value =
                                    "Põhitõed olete juba õppinud, kuid kas olete valmis liikuma järgmisele tasemele?",
                            },
                            new Translation()
                            {
                                Culture = "ru",
                                Value = "Вы уже изучили основы, но готовы ли вы перейти на следующий уровень?",
                            }
                        }
                    },
                    RankTextColor = "#000000",
                    RankColor = "#0066FF",
                    RankIcon = "star;star-half-alt;",
                    MinExperience = 100,
                    MaxExperience = 500
                },
                new Rank()
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000005"),
                    RankTitle = new LangString()
                    {
                        Translations = new List<Translation>()
                        {
                            new Translation()
                            {
                                Culture = "en",
                                Value = "Master",
                            },
                            new Translation()
                            {
                                Culture = "et",
                                Value = "Meister",
                            },
                            new Translation()
                            {
                                Culture = "ru",
                                Value = "Мастер",
                            }
                        }
                    },
                    RankCode = "X_04",
                    RankDescription = new LangString()
                    {
                        Translations = new List<Translation>()
                        {
                            new Translation()
                            {
                                Culture = "en",
                                Value =
                                    "Yes, you feel confidence and even can teach the basics to Newbies. However, there is no limit to perfection C:",
                            },
                            new Translation()
                            {
                                Culture = "et",
                                Value =
                                    "Jah, tunnete enesekindlust ja saate isegi algajatele põhitõdesid õpetada. Täiuslikkusel pole aga piire С:",
                            },
                            new Translation()
                            {
                                Culture = "ru",
                                Value =
                                    "Да, вы чувствуете уверенность и даже можете научить новичков основам. Однако нет предела совершенству C:",
                            }
                        }
                    },
                    RankTextColor = "#000000",
                    RankColor = "#6633FF",
                    RankIcon = "star;star;",
                    MinExperience = 500,
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
                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                    GiftName = new LangString()
                    {
                        Translations = new List<Translation>()
                        {
                            new Translation()
                            {
                                Culture = "en",
                                Value = "Hello :)"
                            },
                            new Translation()
                            {
                                Culture = "et",
                                Value = "Tere :)"
                            },
                            new Translation()
                            {
                                Culture = "ru",
                                Value = "Привет :)"
                            },
                        }
                    },
                    GiftCode = "X_00",
                    Price = 8,
                    GiftImage = new Image()
                    {
                        Id = new Guid("00000000-0000-0000-0000-000000000001"),
                        ImageFor = new Guid("00000000-0000-0000-0000-000000000001"),
                        ImageType = ImageType.Gift,
                        ImageUrl = @"\images\gifts\00000000-0000-0000-0000-000000000001\00000000-0000-0000-0000-000000000001.jpg",
                        OriginalImageUrl = @"\images\gifts\00000000-0000-0000-0000-000000000001\00000000-0000-0000-0000-000000000001.jpg",
                        HeightPx = 236,
                        WidthPx = 236,
                    }
                },
                new Gift()
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    GiftName = new LangString()
                    {
                        Translations = new List<Translation>()
                        {
                            new Translation()
                            {
                                Culture = "en",
                                Value = "Together forever!"
                            },
                            new Translation()
                            {
                                Culture = "et",
                                Value = "Igavesti koos!"
                            },
                            new Translation()
                            {
                                Culture = "ru",
                                Value = "Вместе навсегда!"
                            },
                        }
                    },
                    GiftCode = "X_01",
                    Price = 5,
                    GiftImage = new Image()
                    {
                        Id = new Guid("00000000-0000-0000-0000-000000000002"),
                        ImageFor = new Guid("00000000-0000-0000-0000-000000000002"),
                        ImageType = ImageType.Gift,
                        ImageUrl = @"\images\gifts\00000000-0000-0000-0000-000000000002\00000000-0000-0000-0000-000000000002.jpg",
                        OriginalImageUrl = @"\images\gifts\00000000-0000-0000-0000-000000000002\00000000-0000-0000-0000-000000000002.jpg",
                        HeightPx = 700,
                        WidthPx = 700,
                    }
                },
                new Gift()
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000003"),
                    GiftName = new LangString()
                    {
                        Translations = new List<Translation>()
                        {
                            new Translation()
                            {
                                Culture = "en",
                                Value = "You are cute!"
                            },
                            new Translation()
                            {
                                Culture = "et",
                                Value = "Sa oled armas!"
                            },
                            new Translation()
                            {
                                Culture = "ru",
                                Value = "Ты просто прелесть!"
                            },
                        }
                    },
                    GiftCode = "X_02",
                    Price = 10,
                    GiftImage = new Image()
                    {
                        Id = new Guid("00000000-0000-0000-0000-000000000003"),
                        ImageFor = new Guid("00000000-0000-0000-0000-000000000003"),
                        ImageType = ImageType.Gift,
                        ImageUrl = @"\images\gifts\00000000-0000-0000-0000-000000000003\00000000-0000-0000-0000-000000000003.jpg",
                        OriginalImageUrl = @"\images\gifts\00000000-0000-0000-0000-000000000003\00000000-0000-0000-0000-000000000003.jpg",
                        HeightPx = 512,
                        WidthPx = 512,
                    }
                },
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
            public string UserName { get; set; }
            public string Password { get; set; }

            public ICollection<string>? RolesNames { get; set; }
        }

        struct Role
        {
            public string Name { get; set; }
        }
    }
}