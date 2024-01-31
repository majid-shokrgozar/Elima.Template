using Elima.Common.Domain.Entities;

namespace Elima.Template.FirstModule.Domain.Samples;

public class Sample : Entity<SampleId>
{
    public Sample()
    {
        Id = new SampleId(Guid.NewGuid());
    }
    public string Name { get; set; } = string.Empty;

    public DateOnly CreateDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public TimeOnly CreateTime { get; set; } = TimeOnly.FromDateTime(DateTime.Now);
}


public record SampleId(Guid Value);