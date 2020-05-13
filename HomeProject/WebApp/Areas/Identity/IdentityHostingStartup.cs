using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(WebApp.Areas.Identity.IdentityHostingStartup))]

namespace WebApp.Areas.Identity
{
    /// <inheritdoc />
    public class IdentityHostingStartup : IHostingStartup
    {
        /// <inheritdoc />
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => { });
        }
    }
}