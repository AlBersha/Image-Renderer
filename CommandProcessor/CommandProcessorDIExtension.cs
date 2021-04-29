using Microsoft.Extensions.DependencyInjection;

namespace ConsoleProcessor
{
    public static class CommandProcessorDIExtension
    {
        public static void RegisterCommandProcessor(this IServiceCollection collection)
        {
            collection.AddScoped<ICommandProcessor, ConsoleCommandProcessor>();
        }    
    }
}