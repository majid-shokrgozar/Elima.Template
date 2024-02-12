using Elima.Template.FirstModule.Domain.Samples;
using FastEndpoints;
using FluentValidation;

namespace Elima.Template.FirstModule.Presention.Samples.Create;

public class CreateSampleValidation : Validator<CreateSampleRequest>
{
    public CreateSampleValidation()
    {
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(SampleConsts.NameMaxLength);
    }
}