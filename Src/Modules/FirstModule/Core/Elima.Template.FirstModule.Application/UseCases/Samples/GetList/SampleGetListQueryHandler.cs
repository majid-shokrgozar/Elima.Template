using Elima.Common.Application.MediatR.Queries;
using Elima.Common.Results;
using Elima.Template.FirstModule.Domain.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elima.Template.FirstModule.Application.UseCases.Samples.GetList
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
            var list =await _sampleRepository.GetPagedListAsync(request.SkipCount, request.MaxResultCount, "");
            var count =await _sampleRepository.GetCountAsync();


            var result = new PagedResultDto<SampleDto>(count, list.Select(x => new SampleDto(x.Name)).ToList());

            return PagedResult<SampleDto>.Success(result);
        }
    }
}
