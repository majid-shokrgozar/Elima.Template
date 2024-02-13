using Elima.Common.Modularity;
using Elima.Common.Modularity.Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace DigiPay.Template.CoreModule.Application;

public class DigiPayCoreModuleApplicationModule: ElimaAutofacModule
{
    public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {

        context.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DigiPayCoreModuleApplicationModule).Assembly));

        return Task.CompletedTask;
    }
}
