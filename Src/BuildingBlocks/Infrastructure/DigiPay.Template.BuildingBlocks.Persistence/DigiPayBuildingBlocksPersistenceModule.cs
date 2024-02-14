using Elima.Common.Domain.Entities.Auditing.Contracts;
using Elima.Common.EntityFramework.Auditing;
using Elima.Common.EntityFramework.Data;
using Elima.Common.EntityFramework.EntityFrameworkCore;
using Elima.Common.EntityFramework.Uow;
using Elima.Common.Modularity;
using Elima.Common.Modularity.Autofac;
using Microsoft.Extensions.DependencyInjection;
using Elima.Common.EntityFramework;
using DigiPay.Template.CoreModule.Persistence;
using DigiPay.Template.CoreModule.Persistence.EntityFramework;

namespace DigiPay.Template.BuildingBlocks.Persistence;

[DependsOn(typeof(DigiPayCoreModulePersistenceModule))]
public class DigiPayBuildingBlocksPersistenceModule : ElimaAutofacModule
{
    public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        context.Services.AddUnitOfWork(typeof(SampleModuleDbContext));

        context.Services.AddAuditPropertySetter();

        context.Services.AddDataFilter(options =>
        {
            options.DefaultStates[typeof(ISoftDelete)] = new DataFilterState(isEnabled: true);
        });

        return base.ConfigureServicesAsync(context);
    }
}