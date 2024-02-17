using DigiPay.Template.CoreModule.Application.UseCases.Samples;
using FastEndpoints;
using MediatR;

namespace DigiPay.Template.CoreModule.Presentation.Samples;

public class SampleUpdateByIdEndpoint : EndpointElimaResultWithMapping<SampleUpdateByIdRequest, SampleUpdateByIdResponse, SampleUpdateByIdCommand, SampleDto>
{
    private readonly ISender _sender;

    public SampleUpdateByIdEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Patch("/Samples/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SampleUpdateByIdRequest request, CancellationToken ct)
    {
        var result = await _sender.Send(MapToCommandOrQuery(request), ct);

        await SendResultAsync(result, ct);
    }

    public override SampleUpdateByIdCommand MapToCommandOrQuery(SampleUpdateByIdRequest r)
    {
        return new SampleUpdateByIdCommand(r.Id, r.Name);
    }

    public override SampleUpdateByIdResponse MapToResponse(SampleDto e)
    {
        return new SampleUpdateByIdResponse(e.Id, e.Name);
    }
}
