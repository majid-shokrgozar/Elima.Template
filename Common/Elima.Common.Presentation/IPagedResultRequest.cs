
namespace Elima.Common.Presentation;

public interface IPagedResultRequest : ILimitedResultRequest
{
    /// <summary>
    /// Skip count (beginning of the page).
    /// </summary>
    int SkipCount { get; set; }
}
