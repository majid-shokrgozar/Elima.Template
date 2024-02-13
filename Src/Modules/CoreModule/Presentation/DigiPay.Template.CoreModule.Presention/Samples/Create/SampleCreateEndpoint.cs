using DigiPay.Template.CoreModule.Application.UseCases.Samples;
using FastEndpoints;
using FluentValidation.Results;
using MediatR;

namespace DigiPay.Template.CoreModule.Presention.Samples.Create;

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

    public override async Task HandleAsync(SampleCreateRequest request, CancellationToken ct)
    {
        var result = await _sender.Send(MapToCommandOrQuery(request), ct);
        await SendAsync(result, ct);
    }

    public override Task<SampleCreateResponse> MapToResponseAsync(SampleDto e, CancellationToken ct = default)
    {
        return Task.FromResult(new SampleCreateResponse(e.Name)); 
    }
    public override SampleCreateCommand MapToCommandOrQuery(SampleCreateRequest request)
    {
        return new SampleCreateCommand(request.Name);
    }

}
