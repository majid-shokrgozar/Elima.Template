using Elima.Template.FirstModule.Application.UseCases.Samples;
using FastEndpoints;
using FluentValidation.Results;
using MediatR;

namespace Elima.Template.FirstModule.Presention.Samples.Create;

public class SampleCreateEndpoint : EndpointElimaResultWithMapping<SampleCreateRequest, SampleCreateResponse,SampleCreateCommand, SampleDto>
{
    private readonly ISender _sender;

    public SampleCreateEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Post("/Samples");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SampleCreateRequest req, CancellationToken ct)
    {
        var result = await _sender.Send(MapToCommandOrQuery(req), ct);

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

    public override Task<SampleCreateResponse> MapFromResultValueAsync(SampleDto e, CancellationToken ct = default)
    {
        return Task.FromResult(new SampleCreateResponse(e.Name)); 
    }
    public override SampleCreateCommand MapToCommandOrQuery(SampleCreateRequest request)
    {
        return new SampleCreateCommand(request.Name);
    }

}
