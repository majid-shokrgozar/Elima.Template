using DigiPay.Template.CoreModule.Application.UseCases.Samples;
using FastEndpoints;
using MediatR;

namespace DigiPay.Template.CoreModule.Presentation.Samples;

public class SampleGetByIdEndpoint : EndpointElimaResultWithMapping<SampleGetByIdRequest, SampleGetByIdResponse, SampleGetByIdQuery, SampleDto>
{
    private readonly ISender _sender;

    public SampleGetByIdEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Get("/Samples/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SampleGetByIdRequest request, CancellationToken ct)
    {
        var result = await _sender.Send(MapToCommandOrQuery(request), ct);

        await SendResultAsync(result, ct);
    }

    public override SampleGetByIdQuery MapToCommandOrQuery(SampleGetByIdRequest r)
    {
        return new SampleGetByIdQuery(r.Id);
    }

    public override SampleGetByIdResponse MapToResponse(SampleDto e)
    {
        return new SampleGetByIdResponse(e.Id, e.Name);
    }
}
