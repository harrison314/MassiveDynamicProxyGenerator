using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using MassiveDynamicProxyGenerator.Microsoft.DependencyInjection;
using SampleWebApplication.Services.Contract;
using SampleWebApplication.Services.Implementation;
using SampleWebApplication.Services.Interceptors;

namespace SampleWebApplication
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IArticleService, MockArticleService>();


            services.AddInterceptedDecorator<IArticleService, PerformaceInterceptor>();

            // services.AddDecorator<IArticleService, ArticleDecoratorService>();
            // services.AddInterceptedDecorator<IArticleService, ChangeAutorInterceptor>("Ing interceptor");

            services.AddProxy<INotificationServise>();
            services.AddProxy<ICommonServices, ServiceProviderInterceptor>();

            services.AddMvc();

            return services.BuldIntercepedServiceProvider();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
