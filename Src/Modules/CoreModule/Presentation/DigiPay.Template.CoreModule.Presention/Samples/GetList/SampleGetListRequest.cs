using Elima.Common.Presentation;

namespace DigiPay.Template.CoreModule.Presentation.Samples;

public record SampleGetListRequest : PagedResultRequest
{
    public string? Name { get; set; }
}
