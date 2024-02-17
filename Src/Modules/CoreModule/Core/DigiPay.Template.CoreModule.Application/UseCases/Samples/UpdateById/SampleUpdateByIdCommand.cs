using Elima.Common.Application.MediatR.Commands;

namespace DigiPay.Template.CoreModule.Application.UseCases.Samples;

public record SampleUpdateByIdCommand(Guid Id, string Name) : ICommand<SampleDto>
{
}
