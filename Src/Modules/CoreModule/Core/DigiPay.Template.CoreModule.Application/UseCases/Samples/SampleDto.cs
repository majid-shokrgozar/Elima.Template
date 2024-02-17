using Elima.Common.Application;

namespace DigiPay.Template.CoreModule.Application.UseCases.Samples;

public record SampleDto(Guid Id, string Name):EntityDto<Guid>(Id) {
}

