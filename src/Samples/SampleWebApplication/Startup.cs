using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using SampleWebApplication.IocExtensions;
using SampleWebApplication.Services;
using MassiveDynamicProxyGenerator;
using SampleWebApplication.Services.Interceptors;

namespace SampleWebApplication
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddSingleton<IArticleService, MockArticleService>();

            ProxygGenerator generator = new ProxygGenerator();

            services.AddSingleton<IArticleService>(sp =>
            {
                ILogger<PerformaceInterceptor> logger = sp.GetRequiredService<ILogger<PerformaceInterceptor>>();
                ICallableInterceptor interceptor = new PerformaceInterceptor(logger);

                MockArticleService realService = new MockArticleService();
                IArticleService proxy = generator.GenerateDecorator<IArticleService>(interceptor, realService);

                return proxy;
            });

            //services.Decorate<IArticleService, ArticleDecoratorService>();

           // services.Intercept<IArticleService, ChangeAutorInterceptor>("Ing interceptor");

            services.AddProxy<INotificationServise>();
            services.ImplementCommonServiceProvider<ICommonServices>();

            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseApplicationInsightsRequestTelemetry();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseApplicationInsightsExceptionTelemetry();

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
