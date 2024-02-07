using Elima.Common.ExceptionHandling;
using Elima.Common.Modularity;
using Elima.Common.Modularity.Autofac;
using Elima.Common.Security.Authentication;
using Elima.Template.BuildingBlocks.Presentation.ExceptionHandler;
using Elima.Template.WebApi.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Elima.Template.BuildingBlocks.Presentation;

public class ElimaBuildingBlocksPresentationModule  : ElimaAutofacModule
{
    public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        context.Services.AddHttpContextAccessor();

        return Task.CompletedTask;
    }
}
