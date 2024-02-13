using Elima.Common.Modularity;
using Elima.Common.Modularity.Autofac;
using Microsoft.Extensions.DependencyInjection;

namespace DigiPay.Template.FirstModule.Application;

public class DigiPayFirstModuleApplicationModule: ElimaAutofacModule
{
    public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {

        context.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DigiPayFirstModuleApplicationModule).Assembly));

        return Task.CompletedTask;
    }
}
