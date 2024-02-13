using Azure;
using Elima.Common.Application.MediatR.Queries;
using Elima.Common.Presentation;
using Elima.Common.Results;
using Elima.Template.FirstModule.Application.UseCases.Samples;
using Elima.Template.FirstModule.Application.UseCases.Samples.GetList;
using FastEndpoints;
using MediatR;

namespace Elima.Template.FirstModule.Presention.Samples.GetList;


public record SampleGetListRequest : PagedResultRequest
{
    public string? Name { get; set; }
}

public class SampleGetListEndpoint : EndpointElimaResultWithMapping<SampleGetListRequest, PagedResultResponse<SampleDto>, SampleGetListQuery, PagedResultDto<SampleDto>>
{
    private readonly ISender _sender;

    public SampleGetListEndpoint(ISender sender)
    {
        _sender = sender;
    }

    public override void Configure()
    {
        Get("/Samples");
        AllowAnonymous();
    }

    public async override Task HandleAsync(SampleGetListRequest request, CancellationToken ct)
    {
        var result = await _sender.Send(MapToCommandOrQuery(request), ct);

        await SendAsync(result, ct);
    }

    public override SampleGetListQuery MapToCommandOrQuery(SampleGetListRequest request)
    {
        return new SampleGetListQuery() { MaxResultCount = 10, SkipCount = 0 };
    }

    public override Task<PagedResultResponse<SampleDto>> MapToResponseAsync(PagedResultDto<SampleDto> e, CancellationToken ct = default)
    {
        return Task.FromResult(new PagedResultResponse<SampleDto>(e.TotalCount, e.Items));
    }
}
