using Elima.Common.ExceptionHandling;

namespace DigiPay.Template.BuildingBlocks.Presentation.ExceptionHandler;

public class ProblemDetailsResponse
{
    public ProblemDetails Error { get; set; }

    public ProblemDetailsResponse(ProblemDetails error)
    {
        Error = error;
    }
}
