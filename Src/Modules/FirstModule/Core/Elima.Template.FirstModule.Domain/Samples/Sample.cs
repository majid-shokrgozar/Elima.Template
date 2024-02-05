using Elima.Common.Domain.Entities;
using Elima.Common.Domain.Entities.Auditing;
using Elima.Common.Domain.Entities.Auditing.Contracts;

namespace Elima.Template.FirstModule.Domain.Samples;

public class Sample : FullAuditedAggregateRoot<SampleId>
{
    public Sample():base(new SampleId(Guid.NewGuid()))
    {
    }
    public string Name { get; set; } = string.Empty;
}


public record SampleId(Guid Value);