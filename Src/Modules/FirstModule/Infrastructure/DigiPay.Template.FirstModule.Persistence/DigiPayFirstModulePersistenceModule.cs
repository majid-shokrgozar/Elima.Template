using Elima.Common.EntityFramework.EntityFrameworkCore;
using Elima.Common.Modularity;
using Elima.Common.Modularity.Autofac;
using DigiPay.Template.FirstModule.Domain.Samples;
using DigiPay.Template.FirstModule.Persistence.EntityFramework;
using DigiPay.Template.FirstModule.Persistence.Samples;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DigiPay.Template.FirstModule.Persistence;

public class DigiPayFirstModulePersistenceModule : ElimaAutofacModule
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
