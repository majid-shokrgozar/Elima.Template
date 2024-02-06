using Elima.Common.ExceptionHandling;
using Elima.Common.Modularity;
using Elima.Common.Security.Authentication;
using Elima.Template.BuildingBlocks.Presentation.ExceptionHandler;
using Elima.Template.WebApi.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace Elima.Template.BuildingBlocks.Presentation;

//[DependsOn(typeof(ElimaBuildingBlocksPersistenceModule))]
public class ElimaBuildingBlocksPresentationModule  : ElimaModule
{
    public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        context.Services.AddHttpContextAccessor();

        context.Services.AddScoped<ICurrentUser, CurrentUser>();

        return Task.CompletedTask;
    }
}
