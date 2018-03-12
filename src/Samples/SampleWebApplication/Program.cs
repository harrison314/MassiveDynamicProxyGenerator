using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace SampleWebApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureLogging(loggerBuilder =>
            {
                loggerBuilder.AddFilter((cathegory, _) => cathegory.StartsWith("SampleWebApplication"));
                loggerBuilder.AddConsole();
                loggerBuilder.AddDebug();
            })
                .UseStartup<Startup>()
                .Build();
    }
}
