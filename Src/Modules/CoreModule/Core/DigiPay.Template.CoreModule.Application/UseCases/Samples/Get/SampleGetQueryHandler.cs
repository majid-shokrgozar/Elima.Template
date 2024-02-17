﻿using Ardalis.Specification;
using Azure.Core;
using DigiPay.Template.CoreModule.Domain.Samples;
using Elima.Common.Application.MediatR.Queries;
using Elima.Common.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiPay.Template.CoreModule.Application.UseCases.Samples.Get;

public class SampleGetQueryHandler : IQueryHandler<SampleGetQuery, SampleDto>
{
    private readonly IQuerySampleRepository _sampleRepository;

    public SampleGetQueryHandler(IQuerySampleRepository sampleRepository)
    {
        _sampleRepository = sampleRepository;
    }

    public async Task<Result<SampleDto>> Handle(SampleGetQuery request, CancellationToken cancellationToken)
    {
        var specification = new GetSampleListSpecification(request.Id);

        var sampleDto=await _sampleRepository.GetFirstOrDefaultAsync(specification,cancellationToken);

        return sampleDto;
    }
}

public class GetSampleListSpecification : SingleResultSpecification<Sample,SampleDto>
{
    public GetSampleListSpecification(Guid id)
    {
        Query
            .Select(x => new SampleDto(x.Id.Value, x.Name))
            .Where(x => x.Id.Equals(new SampleId(id)))
            .AsNoTracking();
    }
}