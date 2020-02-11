using Dicer.Model.Dicer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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
