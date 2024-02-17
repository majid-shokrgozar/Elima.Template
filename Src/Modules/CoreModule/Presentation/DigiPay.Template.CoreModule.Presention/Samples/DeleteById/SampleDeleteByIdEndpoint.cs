using DigiPay.Template.CoreModule.Application.UseCases.Samples;
using FastEndpoints;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiPay.Template.CoreModule.Presentation.Samples;

public class SampleDeleteByIdEndpoint : EndpointElimaResultWithMapping<SampleDeleteByIdRequest, SampleDeleteByIdCommand>
{
    private readonly ISender _sender;

    public SampleDeleteByIdEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Delete("/Samples/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SampleDeleteByIdRequest request, CancellationToken ct)
    {
        var result = await _sender.Send(MapToCommandOrQuery(request), ct);

        await SendResultAsync(result, ct);
    }

    public override SampleDeleteByIdCommand MapToCommandOrQuery(SampleDeleteByIdRequest request)
    {
        return new SampleDeleteByIdCommand(request.Id);
    }
}
