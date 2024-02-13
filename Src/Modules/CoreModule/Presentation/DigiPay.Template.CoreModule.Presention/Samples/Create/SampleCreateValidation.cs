using DigiPay.Template.CoreModule.Domain.Samples;
using FastEndpoints;
using FluentValidation;

namespace DigiPay.Template.CoreModule.Presention.Samples.Create;

public class SampleCreateValidation : Validator<SampleCreateRequest>
{
    public SampleCreateValidation()
    {
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(SampleConsts.NameMaxLength);
    }
}