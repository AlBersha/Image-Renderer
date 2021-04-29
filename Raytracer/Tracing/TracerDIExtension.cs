using Microsoft.Extensions.DependencyInjection;

namespace Raytracer.Tracing
{
    public static class TracerDIExtension
    {
        public static void RegisterTracer(this IServiceCollection collection)
        {
            collection.AddScoped<ITracer, Raytracer>();
        }    
    }
}