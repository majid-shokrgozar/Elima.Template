using System.Reflection;

namespace Elima.Common.Modularity;

public interface IElimaModule
{
    Assembly ThisAssembly { get; }

    Task ConfigureServicesAsync(ServiceConfigurationContext context);
    Task BaseConfigureServicesAsync(ServiceConfigurationContext context);
    Task OnApplicationInitializationAsync(ApplicationInitializationContext context);
    Task PostConfigureServicesAsync(ServiceConfigurationContext context);
    Task PreConfigureServicesAsync(ServiceConfigurationContext context);
}
