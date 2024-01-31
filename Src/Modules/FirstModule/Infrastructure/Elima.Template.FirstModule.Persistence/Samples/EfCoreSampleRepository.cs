using Elima.Common.EntityFramework.Repositories;
using Elima.Template.FirstModule.Domain.Samples;
using Elima.Template.FirstModule.Persistence.EntityFramework;

namespace Elima.Template.FirstModule.Persistence.Samples
{
    public class EfCoreSampleRepository : EfCoreRepository<SampleModuleDbContext, Sample>, ISampleRepository
    {
        public EfCoreSampleRepository(SampleModuleDbContext dbContext) : base(dbContext)
        {
        }
    }
}
