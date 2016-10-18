using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Bangazon.Data;
using Microsoft.EntityFrameworkCore;

namespace BangazonAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Console.WriteLine("Startup");
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Console.WriteLine("ConfigureServices");
            // Add framework services.
            services.AddMvc();

            // Add CORS framework
            services.AddCors(options =>
            {
                options.AddPolicy("AllowDevelopmentEnvironment",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            //AddCors - this is opening it up to everyone and anyone can access this in any way


            //this is setting up the DB 
            // to look up the environment on your computer type "env" into the command line. These are all variables with values. 
            //PATH what directories should I look for to find the file that you want
            //Environment variable becuase there is a username in the environment path on our local machines
            string path = System.Environment.GetEnvironmentVariable("Bangazon_Db_Path");
            var connection = $"Filename={path}";
            Console.WriteLine($"connection = {connection}");
            services.AddDbContext<BangazonContext>(options => options.UseSqlite(connection));
            //Sqlite - eventually UseSqlServer when projects get more intense
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            Console.WriteLine("Configure");
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}



// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Logging;

// namespace BangazonAPI
// {
//     public class Startup
//     {
//         public Startup(IHostingEnvironment env)
//         {
//             //environments can have different properties
//             var builder = new ConfigurationBuilder()
//                 .SetBasePath(env.ContentRootPath)
//                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//                 .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
//                 .AddEnvironmentVariables();
//             Configuration = builder.Build();
//         }

//         public IConfigurationRoot Configuration { get; }

//         // This method gets called by the runtime. Use this method to add services to the container.
//         public void ConfigureServices(IServiceCollection services)
//         {
//             // Add framework services.
//             services.AddMvc();
//             //looks for anythign that is a controller
//         }

//         // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
//         public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
//         {
//             loggerFactory.AddConsole(Configuration.GetSection("Logging"));
//             loggerFactory.AddDebug();

//             app.UseMvc();
//         }
//     }
// }

// //middleware is the middle - what happens in the middle - logic, responses - app.UseMvc() which makes it a fullblown Mvc application
