using Elima.Common.DependencyInjection;
using Elima.Common.EntityFramework.Repositories;
using DigiPay.Template.CoreModule.Domain.Samples;
using DigiPay.Template.CoreModule.Persistence.EntityFramework;
using Elima.Common.EntityFramework.EntityFrameworkCore;

namespace DigiPay.Template.CoreModule.Persistence.Samples
{
    public class EfCoreSampleRepository : CommandRepository<SampleModuleDbContext, Sample>, ISampleRepository,ITransientDependency
    {
        public EfCoreSampleRepository(SampleModuleDbContext dbContext) : base(dbContext)
        {
        }
    }
}
