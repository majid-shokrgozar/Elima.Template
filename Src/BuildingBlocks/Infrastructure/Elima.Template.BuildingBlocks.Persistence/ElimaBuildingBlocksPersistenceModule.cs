using Elima.Common.EntityFramework.EntityFrameworkCore;
using Elima.Common.EntityFramework.Uow;
using Elima.Common.Modularity;
using Elima.Template.FirstModule.Persistence;
using Elima.Template.FirstModule.Persistence.EntityFramework;
using Microsoft.Extensions.DependencyInjection;

namespace Elima.Template.BuildingBlocks.Persistence;

[DependsOn(typeof(ElimaFirstModulePersistenceModule))]
public class ElimaBuildingBlocksPersistenceModule : ElimaModule
{
    public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        context.Services.AddScoped<IUnitOfWork>((serviceProvider) =>
        {
            var list = new List<IEfCoreDbContext>();

            var sampleModuleDbContext = serviceProvider.GetService<SampleModuleDbContext>();

            if (sampleModuleDbContext is not null)
                list.Add(sampleModuleDbContext);

            return new UnitOfWork(list);
        });
        return base.ConfigureServicesAsync(context);
    }
}