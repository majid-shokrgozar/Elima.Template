using Elima.Common.Modularity;
using Elima.Template.FirstModule.Domain.Samples;
using Elima.Template.FirstModule.Persistence.EntityFramework;
using Elima.Template.FirstModule.Persistence.Samples;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elima.Template.FirstModule.Persistence;

public class ElimaFirstModulePersistenceModule : ElimaModule
{

    public override Task ConfigureServicesAsync(ServiceConfigurationContext context)
    {
        context.Services.AddDbContext<SampleModuleDbContext>(option =>
        {
            option.UseSqlServer(context.Configuration.GetConnectionString("Default"));
        });

        context.Services.AddTransient<ISampleRepository, EfCoreSampleRepository>();
        return base.ConfigureServicesAsync(context);
    }
}
