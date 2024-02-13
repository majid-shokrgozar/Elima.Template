using Elima.Common.Modularity;
using Elima.Common.Modularity.Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace Elima.Template.FirstModule.Application;

public class ElimaFirstModuleApplicationModule: ElimaAutofacModule
{
    public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {

        context.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ElimaFirstModuleApplicationModule).Assembly));

        return Task.CompletedTask;
    }
}
