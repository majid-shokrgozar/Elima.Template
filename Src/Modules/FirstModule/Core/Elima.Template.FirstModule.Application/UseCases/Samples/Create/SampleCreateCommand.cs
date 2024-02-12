using Elima.Common.Application.MediatR.Commands;
using Elima.Template.FirstModule.Domain.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elima.Template.FirstModule.Application.UseCases.Samples;

public record SampleCreateCommand(string Name):ICommand<SampleDto>
{
}
