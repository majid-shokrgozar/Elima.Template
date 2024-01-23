using Elima.Common.Collections;

namespace Elima.Common.SimpleStateChecking;

public class SimpleStateCheckerOptions<TState>
    where TState : IHasSimpleStateCheckers<TState>
{
    public ITypeList<ISimpleStateChecker<TState>> GlobalStateCheckers { get; }

    public SimpleStateCheckerOptions()
    {
        GlobalStateCheckers = new TypeList<ISimpleStateChecker<TState>>();
    }
}
