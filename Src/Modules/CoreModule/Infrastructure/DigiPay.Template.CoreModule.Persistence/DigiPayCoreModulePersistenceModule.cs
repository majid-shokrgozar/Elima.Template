using Elima.Common.EntityFramework.EntityFrameworkCore;
using Elima.Common.Modularity;
using Elima.Common.Modularity.Autofac;
using DigiPay.Template.CoreModule.Domain.Samples;
using DigiPay.Template.CoreModule.Persistence.EntityFramework;
using DigiPay.Template.CoreModule.Persistence.Samples;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigiPay.Template.CoreModule.Persistence;

public class DigiPayCoreModulePersistenceModule : ElimaAutofacModule
{

    public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        context.Services.AddDbContext<SampleModuleDbContext>(option =>
        {
            option.UseSqlServer(context.Configuration.GetConnectionString("Default"));
        });
        return base.ConfigureServicesAsync(context);
    }
}
