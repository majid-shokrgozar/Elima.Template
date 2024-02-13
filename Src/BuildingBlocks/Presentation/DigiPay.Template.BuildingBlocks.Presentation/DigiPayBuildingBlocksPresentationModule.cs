using Elima.Common.ExceptionHandling;
using Elima.Common.Modularity;
using Elima.Common.Modularity.Autofac;
using Elima.Common.Security.Authentication;
using DigiPay.Template.BuildingBlocks.Presentation.ExceptionHandler;
using DigiPay.Template.WebApi.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace DigiPay.Template.BuildingBlocks.Presentation;

public class DigiPayBuildingBlocksPresentationModule  : ElimaAutofacModule
{
    public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        context.Services.AddHttpContextAccessor();

        return Task.CompletedTask;
    }
}
