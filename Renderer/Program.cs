using System;
using System.Threading.Tasks;
using ConsoleProcessor;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Raytracer.ObjectProvider;
using Raytracer.Optimisation;
using Raytracer.Scene;
using Raytracer.Tracing;

namespace Renderer
{
    internal static class Program
    {
        private static Task Main(string[] args)
        {
            args = new[]
            {
                "--source=C:\\Users\\obers\\KPI\\graphics\\cow.obj",
                "--output=C:\\Users\\obers\\KPI\\graphics\\output.ppm"
            };
            
            using var host = CreateHostBuilder(args).Build();
            
            ExemplifyScoping(host.Services, args);
            
            return host.RunAsync();

        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    services.RegisterCommandProcessor();
                    services.RegisterTree();
                    services.RegisterScene();
                    services.RegisterObjectProvider();
                    services.RegisterTracer();
                    services.AddTransient<Startup>();
                });

        private static void ExemplifyScoping(IServiceProvider services, string[] args)
        {
            using var serviceScope = services.CreateScope();
            var provider = serviceScope.ServiceProvider;

            var startup = provider.GetService<Startup>();
            startup?.ConfigureExecutor(args);
        }
    }
}
