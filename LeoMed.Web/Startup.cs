using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using LeoMed.Model;
using LeoMed.Web.Services;

namespace LeoMed.Web
{
     public class Startup
     {
          public IConfigurationRoot Configuration { get; }

          public Startup(IHostingEnvironment env)
          {
               var builder = new ConfigurationBuilder()
                   .SetBasePath(env.ContentRootPath)
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                   .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

               if (env.IsDevelopment())
               {
                    builder.AddUserSecrets<Startup>();
               }

               builder.AddEnvironmentVariables();
               Configuration = builder.Build();
          }

          // This method gets called by the runtime. Use this method to add services to the container.
          public void ConfigureServices(IServiceCollection services)
          {
               // Add framework services.
               services.AddDbContext<AppDbContext>(options =>
                   options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

               services.AddIdentity<AppUser, IdentityRole>()
                   .AddEntityFrameworkStores<AppDbContext>()
                   .AddDefaultTokenProviders();

               services.Configure<IdentityOptions>(options =>
               {
                    // Password settings
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;

                    // Lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                    options.Lockout.MaxFailedAccessAttempts = 10;

                    // Cookie settings
                    options.Cookies.ApplicationCookie.ExpireTimeSpan = TimeSpan.FromDays(150);
                    options.Cookies.ApplicationCookie.LoginPath = "/";
                    options.Cookies.ApplicationCookie.LogoutPath = "/Account/LogOff";

                    // User settings
                    options.User.RequireUniqueEmail = true;
               });

               services.AddMvc();

               // Add application services.
               services.AddTransient<IEmailSender, AuthMessageSender>();
               services.AddTransient<ISmsSender, AuthMessageSender>();
          }

          // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
          public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
          {
               loggerFactory.AddConsole(Configuration.GetSection("Logging"));
               loggerFactory.AddDebug();

               if (env.IsDevelopment())
               {
                    app.UseDeveloperExceptionPage();
                    //app.UseDatabaseErrorPage();
                    app.UseBrowserLink();
               }
               else
               {
                    app.UseExceptionHandler("/Home/Error");
               }

               app.UseStaticFiles();

               app.UseIdentity();

               // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

               app.UseFacebookAuthentication(new FacebookOptions()
               {
                    AppId = "3423",
                    AppSecret = "3324233333fefcdcdcdcdcdcc33"
               });

               app.UseGoogleAuthentication(new GoogleOptions()
               {
                    ClientId = "dscdjc3efjdcjdk33jedndcmdc",
                    ClientSecret = "scjkscs7326333scsc"
               });

               app.UseMicrosoftAccountAuthentication(new MicrosoftAccountOptions()
               {
                    ClientId = "qdhshc73edhdjc37dweydc",
                    ClientSecret = "dcdmcdjcu8dcydchdc"
               });

               app.UseTwitterAuthentication(new TwitterOptions()
               {
                    ConsumerKey = "skjschjschsc",
                    ConsumerSecret = "sskscuolscuid7c8dj"
               });

               app.UseMvc(routes =>
               {
                    routes.MapRoute(
                         name: "areaRoute",
                         template: "{area:exists}/{controller=Home}/{action=Index}/{tag?}"
                         );

                    routes.MapRoute(
                         name: "default",
                         template: "{controller=Home}/{action=Index}/{tag?}"
                         );
               });
          }
     }
}
