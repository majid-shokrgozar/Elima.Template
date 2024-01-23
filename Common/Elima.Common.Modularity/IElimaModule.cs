namespace Elima.Common.Modularity;

public interface IElimaModule
{
    public Task ConfigureServicesAsync(ServiceConfigurationContext context);

    public Task OnApplicationInitializationAsync(ApplicationInitializationContext context);
}
