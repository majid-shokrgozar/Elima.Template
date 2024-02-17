using Elima.Common.Application.MediatR.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigiPay.Template.CoreModule.Application.UseCases.Samples;

public record SampleGetQuery(Guid Id) : IQuery<SampleDto>
{
}
