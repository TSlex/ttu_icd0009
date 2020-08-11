using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using BLL.App;
using Contracts.BLL.App;
using Contracts.DAL.App;
using DAL.App;
using DAL.App.EF;
using Domain.Identity;
using ee.itcollege.aleksi.Contracts.DAL.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApp.Helpers;

namespace WebApp
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <inheritdoc />
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container. 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options
                    .EnableSensitiveDataLogging() // for testing only
                    .UseMySql(Configuration.GetConnectionString("MySqlConnection")));
            
            services.AddIdentity<AppUser, AppRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddDefaultUI()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            
            services.AddScoped<IUserNameProvider, UserNameProvider>();
            services.AddScoped<IAppUnitOfWork, AppUnitOfWork>();
            services.AddScoped<IAppBLL, AppBLL>();
            
            services.AddRouting(options => options.LowercaseUrls = true);
            
            services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            
            services.AddCors(options =>
            {
                options.AddPolicy("CorsAllowAll",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            
            // =============== JWT support ===============
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication()
                .AddCookie(options => { options.SlidingExpiration = true; })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["JWT:Issuer"],
                        ValidAudience = Configuration["JWT:Issuer"],
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:SigningKey"])),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });
            
            // =============== i18n support ===============
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo(name: "en-GB"),
//                    new CultureInfo(name: "et-EE"),
//                    new CultureInfo(name: "ru-RU")
                };

                // State what the default culture for your application is. 
                options.DefaultRequestCulture = new RequestCulture(culture: "en", uiCulture: "en");

                // You must explicitly state which cultures your application supports.
                options.SupportedCultures = supportedCultures;

                // These are the cultures the app supports for UI strings
                options.SupportedUICultures = supportedCultures;
            });
            
            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
            });
            
            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(
                options => options.ResolveConflictingActions(enumerable => enumerable.First())
            );
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="provider"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            UpdateDatabase(app, env, Configuration);
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            //i18n
            app.UseRequestLocalization(
                options: app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>().Value);

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            
            app.UseCors("CorsAllowAll");

            app.UseRouting();
            
            app.UseSwagger();
            app.UseSwaggerUI(
                options =>
                {
                    foreach (var description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint(
                            $"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                    }
                });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapRazorPages();
            });
        }
        
        private static void UpdateDatabase(IApplicationBuilder app, IWebHostEnvironment env,
            IConfiguration configuration)
        {
            // give me the scoped services (everyhting created by it will be closed at the end of service scope life).
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            using var ctx = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            using var userManager = serviceScope.ServiceProvider.GetService<UserManager<AppUser>>();
            using var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<AppRole>>();
            
            var logger = serviceScope.ServiceProvider.GetService<ILogger<Startup>>();

            if (configuration["AppDataInitialization:DropDatabase"] == "True")
            {
                logger.LogInformation("DropDatabase");
                DAL.App.EF.Helpers.DataInitializers.DeleteDatabase(ctx);
            }

            if (configuration["AppDataInitialization:MigrateDatabase"] == "True")
            {
                logger.LogInformation("MigrateDatabase");
                DAL.App.EF.Helpers.DataInitializers.MigrateDatabase(ctx);
            }

            if (configuration.GetValue<bool>("AppDataInitialization:SeedData"))
            {
                logger.LogInformation("SeedData");
                DAL.App.EF.Helpers.DataInitializers.SeedData(ctx);
            }

            if (configuration["AppDataInitialization:SeedIdentity"] == "True")
            {
                logger.LogInformation("SeedIdentity");
                DAL.App.EF.Helpers.DataInitializers.SeedIdentity(userManager, roleManager);
            }
        }
    }
}