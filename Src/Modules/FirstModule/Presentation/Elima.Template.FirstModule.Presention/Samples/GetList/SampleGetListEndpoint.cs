using Elima.Common.Application.MediatR.Queries;
using Elima.Common.Presentation;
using Elima.Template.FirstModule.Application.UseCases.Samples;
using Elima.Template.FirstModule.Application.UseCases.Samples.GetList;
using FastEndpoints;

namespace Elima.Template.FirstModule.Presention.Samples.GetList;

public class SampleGetListEndpoint: EndpointElimaResultWithMapping<SampleGetListReuqest, SampleGetListResponse, SampleGetListQuery, SampleDto>
{
}

public record SampleGetListReuqest(string? Name): PagedResultRequest
{
    
}

public record SampleGetListResponse(string? Name)
{

}
