using Elima.Common.DependencyInjection;
using Elima.Common.EntityFramework.Repositories;
using DigiPay.Template.CoreModule.Domain.Samples;
using DigiPay.Template.CoreModule.Persistence.EntityFramework;
using Elima.Common.EntityFramework.EntityFrameworkCore;
using Elima.Common.EntityFramework.Specification.Repositories;
using Ardalis.Specification;

namespace DigiPay.Template.CoreModule.Persistence.Samples
{
    public class SampleCommandRepository : SpecificationCommandRepository<SampleModuleDbContext, Sample, SampleId>, ICommandSampleRepository,ITransientDependency
    {
        public SampleCommandRepository(SampleModuleDbContext dbContext) : base(dbContext)
        {
        }
    }

    public class SampleQueryRepository : SpecificationQueryRepository<SampleModuleDbContext, Sample, SampleId>, IQuerySampleRepository, ITransientDependency
    {
        public SampleQueryRepository(SampleModuleDbContext dbContext) : base(dbContext)
        {
        }
    }
}
