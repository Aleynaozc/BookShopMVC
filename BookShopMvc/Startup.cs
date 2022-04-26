using BookShopMvc.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopMvc
{
    public class Startup
    {
        public IConfiguration Configuration {get;} //uygulama ba�latma
          public Startup(IConfiguration configuration) 
    {
          Configuration =  configuration;
    }
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();//MVC olarak ekledik.

            services.AddDbContext<BookContext>(options => options.UseSqlServer(Configuration.GetConnectionString("BookDatabase")));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting(); //Controller ile haberle�tirme i�in kullan�l�r.
            app.UseStaticFiles(); //Static dosyalar� (wwwroot) etkinle�tiriyor.

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Book}/{action=Index}/{id?}"); //Action:A��l�� sayfas�nda hangisini g�rce�imizi yaz�yoruz.
            });
        }
    }
}
