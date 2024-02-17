using Elima.Common.Application.MediatR.Commands;

namespace DigiPay.Template.CoreModule.Application.UseCases.Samples;

public record SampleDeleteByIdCommand(Guid Id) : ICommand
{
}
