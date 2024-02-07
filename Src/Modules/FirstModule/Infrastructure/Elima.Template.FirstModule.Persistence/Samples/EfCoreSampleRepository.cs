using Elima.Common.DependencyInjection;
using Elima.Common.EntityFramework.Repositories;
using Elima.Template.FirstModule.Domain.Samples;
using Elima.Template.FirstModule.Persistence.EntityFramework;

namespace Elima.Template.FirstModule.Persistence.Samples
{
    public class EfCoreSampleRepository : EfCoreRepository<SampleModuleDbContext, Sample>, ISampleRepository,ITransientDependency
    {
        public EfCoreSampleRepository(SampleModuleDbContext dbContext) : base(dbContext)
        {
        }
    }
}
