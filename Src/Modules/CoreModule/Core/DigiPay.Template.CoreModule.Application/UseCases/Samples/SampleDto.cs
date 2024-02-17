using DigiPay.Template.CoreModule.Domain.Samples;
using Elima.Common.Application;

namespace DigiPay.Template.CoreModule.Application.UseCases.Samples;

public record SampleDto(Guid Id, string Name):EntityDto<Guid>(Id) {

    public static SampleDto MapFromEntity(Sample sample)
        => new(sample.Id.Value, sample.Name);
}

