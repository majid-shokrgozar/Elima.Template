using Elima.Common.EntityFramework.Specification.Repositories;

namespace DigiPay.Template.CoreModule.Domain.Samples;

public interface ICommandSampleRepository : ISpecificationCommandRepository<Sample, SampleId>
{
}
