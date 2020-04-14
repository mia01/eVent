using System;
using eventapp.Areas.Identity.Data;
using eventapp.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(eventapp.Areas.Identity.IdentityHostingStartup))]
namespace eventapp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<EventAppContext>(options =>
                    options.UseMySql(
                        context.Configuration.GetConnectionString("EventappConnection")));

                services.AddDefaultIdentity<EventAppUser>(options => {
                    options.SignIn.RequireConfirmedAccount = true;
                    // Default Password settings.
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;
                    options.SignIn.RequireConfirmedPhoneNumber = true;
                    options.User.RequireUniqueEmail = true;
                })
                    .AddEntityFrameworkStores<EventAppContext>();
            });
        }
    }
}