using DigiPay.Template.CoreModule.Domain.Samples;
using Ardalis.Specification;

namespace DigiPay.Template.CoreModule.Application.UseCases.Samples.GetList;

public class GetSampleListSpecification : Specification<Sample, SampleDto>
{
    public GetSampleListSpecification(SampleGetListQuery request)
    {
        Query
            .Select(x => SampleDto.MapFromEntity(x))
            .Skip(request.SkipCount)
            .Take(request.MaxResultCount)
            .Where(x => x.Name.Equals(request.Name),!string.IsNullOrEmpty(request.Name))
            .AsNoTracking();
    }
}