using Elima.Common.Domain.Entities.Auditing.Contracts;
using Elima.Common.EntityFramework.Auditing;
using Elima.Common.EntityFramework.Data;
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

        context.Services.AddSingleton(typeof(IDataFilter), typeof(DataFilter));
        //context.Services.AddSingleton(typeof(IDataFilter<ISoftDelete>), typeof(DataFilter<ISoftDelete>));
        context.Services.AddSingleton(typeof(IDataFilter<>), typeof(DataFilter<>));

        context.Services.AddScoped<IAuditPropertySetter, AuditPropertySetter>();

        context.Services.Configure<DataFilterOptions>(options =>
        {
            options.DefaultStates[typeof(ISoftDelete)] = new DataFilterState(isEnabled: true);
        });

        return base.ConfigureServicesAsync(context);
    }
}