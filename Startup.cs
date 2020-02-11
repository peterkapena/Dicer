using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Dicer.Model.Dicer;

namespace Dicer
{
    public class Startup
    {
        #region "Properties"
        public IConfiguration Configuration { get; }
        #endregion

        #region "Constructors"
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        #endregion


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options => { options.EnableEndpointRouting = false; });
            
            services.AddDbContext<DicerContext>(options => options.UseSqlServer(new Data(Configuration).CnString));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseStatusCodePages();

            //Routing
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                name: "default",
                template: "{controller=Main}/{action=OnRequest}/{id?}");
            });
        }
    }
}
