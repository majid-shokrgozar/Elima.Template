using Azure;
using Elima.Common.Application.MediatR.Queries;
using Elima.Common.Results;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiPay.Template.CoreModule.Application.UseCases.Samples.GetList;

public record SampleGetListQuery(string? Name) : PagedResultQuery<SampleDto>
{
}
