using Elima.Common.Application.MediatR.Queries;
using Elima.Common.Results;
using DigiPay.Template.CoreModule.Domain.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiPay.Template.CoreModule.Application.UseCases.Samples.GetList
{
    public class SampleGetListQueryHandler : IPagedQueryHandler<SampleGetListQuery, SampleDto>
    {
        private readonly ISampleRepository _sampleRepository;

        public SampleGetListQueryHandler(ISampleRepository sampleRepository)
        {
            this._sampleRepository = sampleRepository;
        }

        public async Task<PagedResult<SampleDto>> Handle(SampleGetListQuery request, CancellationToken cancellationToken)
        {
            var list = new List<Sample>();// await _sampleRepository.GetPagedListAsync(request.SkipCount, request.MaxResultCount, "");
            var count = 0;// await _sampleRepository.GetCountAsync();

            return new PagedResultDto<SampleDto>(count, list.Select(x => new SampleDto(x.Name)).ToList());
        }
    }
}
