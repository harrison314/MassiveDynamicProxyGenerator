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
using Microsoft.Extensions.Hosting;

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

            services.AddProxy<INotificationService>();
            services.AddProxy<ICommonServices, ServiceProviderInterceptor>();

            services.AddControllers();

            return services.BuildIntercepedServiceProvider();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
