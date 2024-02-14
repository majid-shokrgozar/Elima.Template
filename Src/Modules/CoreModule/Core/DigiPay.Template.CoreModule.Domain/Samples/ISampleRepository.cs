using Elima.Common.EntityFramework.EntityFrameworkCore;
using Elima.Common.EntityFramework.Repositories;

namespace DigiPay.Template.CoreModule.Domain.Samples;

public interface ISampleRepository : ICommandRepository<IEfCoreDbContext, Sample>
{
}
