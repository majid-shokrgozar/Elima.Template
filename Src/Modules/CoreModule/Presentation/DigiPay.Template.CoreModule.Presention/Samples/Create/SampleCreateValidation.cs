using DigiPay.Template.CoreModule.Domain;
using DigiPay.Template.CoreModule.Domain.Samples;
using FastEndpoints;
using FluentValidation;

namespace DigiPay.Template.CoreModule.Presentation.Samples.Create;

public class SampleCreateValidation : Validator<SampleCreateRequest>
{
    public SampleCreateValidation()
    {
        RuleFor(x => x.Name).NotEmpty().NotNull().MaximumLength(SampleConsts.NameMaxLength);
    }
}