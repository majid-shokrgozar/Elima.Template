using Elima.Common.ExceptionHandling;

namespace Elima.Template.BuildingBlocks.Presentation.ExceptionHandler;

public class ProblemDetailsResponse
{
    public ProblemDetails Error { get; set; }

    public ProblemDetailsResponse(ProblemDetails error)
    {
        Error = error;
    }
}
