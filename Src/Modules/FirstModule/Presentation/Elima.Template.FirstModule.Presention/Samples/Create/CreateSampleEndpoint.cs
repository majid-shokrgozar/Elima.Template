using Elima.Common.Results;
using Elima.Template.FirstModule.Application.UseCases.Samples;
using FastEndpoints;
using FluentValidation.Results;
using MediatR;

namespace Elima.Template.FirstModule.Presention.Samples.Create;

public class CreateSampleEndpoint : EndpointElimaResultWithMapping<CreateSampleRequest, CreateSampleResponse,CreateSampleCommand, SampleDto>
{
    private readonly ISender _sender;

    public CreateSampleEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Post("/Samples");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateSampleRequest req, CancellationToken ct)
    {
        var result = await _sender.Send(MapToCommand(req), ct);

        if (ValidationFailed)
        {
            foreach (ValidationFailure failure in ValidationFailures)
            {
                var propertyName = failure.PropertyName;
                var errorMessage = failure.ErrorMessage;
            }
            await SendErrorsAsync(400, ct);
        }
        await SendAsync(result, ct);
    }

    public override Task<CreateSampleResponse> MapFromResultValueAsync(SampleDto e, CancellationToken ct = default)
    {
        return Task.FromResult(new CreateSampleResponse(e.Name)); 
    }
    public override CreateSampleCommand MapToCommand(CreateSampleRequest request)
    {
        return new CreateSampleCommand(request.Name);
    }

}
