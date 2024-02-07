namespace Elima.Common.Modularity;

public abstract class ElimaModule : BasicElimaModule, IElimaModule
{
    public Task BaseConfigureServicesAsync(ServiceConfigurationContext context)
    {
        return Task.CompletedTask;
    }
}

