using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DataLibrary.DB;
using DataLibrary.Data;

namespace RPDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container. Register dependancies here usin dependancy injection
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            //connecting to db
            services.AddSingleton(new ConnectionStringData 
            { 
                SqlConnectionName = "Default"
            });
            //specifying service & it's implementation to use whenever someone in this solution asks for the instance of Idata access, 
            //they'll get back an instance of sqlDb - news it up and puts it into the interface type
            //if decided to change db provider e.g. from SQL server to mySql all you have to do in this project is change the SqlDb class to return that implementation instead
            services.AddSingleton<IDataAccess, SqlDb>();
            services.AddSingleton<IFoodData, FoodData>(); //these two require the connection string and IData access data
            services.AddSingleton<IOrderData, OrderData>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
