using System.Threading.Tasks;

namespace Elima.Common.SimpleStateChecking;

public interface ISimpleStateChecker<TState>
    where TState : IHasSimpleStateCheckers<TState>
{
    Task<bool> IsEnabledAsync(SimpleStateCheckerContext<TState> context);
}