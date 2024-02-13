using Elima.Common.DependencyInjection;
using Elima.Common.EntityFramework.Repositories;
using DigiPay.Template.FirstModule.Domain.Samples;
using DigiPay.Template.FirstModule.Persistence.EntityFramework;

namespace DigiPay.Template.FirstModule.Persistence.Samples
{
    public class EfCoreSampleRepository : EfCoreRepository<SampleModuleDbContext, Sample>, ISampleRepository,ITransientDependency
    {
        public EfCoreSampleRepository(SampleModuleDbContext dbContext) : base(dbContext)
        {
        }
    }
}
