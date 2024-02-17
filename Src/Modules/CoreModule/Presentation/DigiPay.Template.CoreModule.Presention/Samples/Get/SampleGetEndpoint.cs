using DigiPay.Template.CoreModule.Application.UseCases.Samples;
using FastEndpoints;
using MediatR;

namespace DigiPay.Template.CoreModule.Presentation.Samples;

public class SampleGetEndpoint : EndpointElimaResultWithMapping<SampleGetRequest, SampleGetResponse, SampleGetQuery, SampleDto>
{
    private readonly ISender _sender;

    public SampleGetEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Get("/Samples/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SampleGetRequest request, CancellationToken ct)
    {
        var result = await _sender.Send(MapToCommandOrQuery(request), ct);

        await SendAsync(result, ct);
    }

    public override SampleGetQuery MapToCommandOrQuery(SampleGetRequest r)
    {
        return new SampleGetQuery(r.Id);
    }

    public override SampleGetResponse MapToResponse(SampleDto e)
    {
        return new SampleGetResponse(e.Id, e.Name);
    }
}
