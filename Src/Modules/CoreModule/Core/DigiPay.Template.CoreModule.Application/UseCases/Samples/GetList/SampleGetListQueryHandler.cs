using Elima.Common.Application.MediatR.Queries;
using Elima.Common.Results;
using DigiPay.Template.CoreModule.Domain.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiPay.Template.CoreModule.Application.UseCases.Samples.GetList;

public class SampleGetListQueryHandler : IPagedQueryHandler<SampleGetListQuery, SampleDto>
{
    private readonly IQuerySampleRepository _sampleRepository;

    public SampleGetListQueryHandler(IQuerySampleRepository sampleRepository)
    {
        this._sampleRepository = sampleRepository;
    }

    public async Task<PagedResult<SampleDto>> Handle(SampleGetListQuery request, CancellationToken cancellationToken)
    {
        var spect = new GetSampleListSpecification(request);

        var list = await _sampleRepository.GetListAsync(spect, cancellationToken);

        var count = await _sampleRepository.GetCountAsync(spect, cancellationToken);

        return new PagedResultDto<SampleDto>(count, list);
    }
}
