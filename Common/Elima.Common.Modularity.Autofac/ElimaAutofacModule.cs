
using Microsoft.Extensions.Hosting;
using Autofac;
using Elima.Common.DependencyInjection;

namespace Elima.Common.Modularity.Autofac;

public abstract class ElimaAutofacModule : BasicElimaModule, IElimaModule
{
    public Task BaseConfigureServicesAsync(ServiceConfigurationContext context)
    {
        context.Host.ConfigureContainer<ContainerBuilder>(builder => {
            builder.RegisterAssemblyTypes(ThisAssembly)
            .Where(x => x.GetInterfaces()
            .Contains(typeof(ITransientDependency)))
            .AsImplementedInterfaces()
            .InstancePerDependency();
        });

        context.Host.ConfigureContainer<ContainerBuilder>(builder => {
            builder.RegisterAssemblyTypes(ThisAssembly)
            .Where(x => x.GetInterfaces()
            .Contains(typeof(IScopedDependency)))
            .AsImplementedInterfaces()
            .InstancePerRequest();
        });

        context.Host.ConfigureContainer<ContainerBuilder>(builder => {
            builder.RegisterAssemblyTypes(ThisAssembly)
            .Where(x => x.GetInterfaces()
            .Contains(typeof(ISingletonDependency)))
            .AsImplementedInterfaces()
            .SingleInstance();
        });

        return Task.CompletedTask;
    }
}