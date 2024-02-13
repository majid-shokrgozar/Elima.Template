using Azure;
using Elima.Common.Application.MediatR.Queries;
using Elima.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elima.Template.FirstModule.Application.UseCases.Samples.GetList;

public record SampleGetListQuery : PagedResultQuery<SampleDto>
{
    public SampleGetListQuery()
    {
    }
}
