using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.Core
{
    public abstract class StateEstimator
    {
        public abstract Estimate GetEstimate(State state);
    }

    public abstract class StateEstimator2<TState> : StateEstimator
        where TState : State
    {
        public sealed override Estimate GetEstimate(State state)
        {
            return GetEstimate2(state as TState);
        }

        public abstract Estimate GetEstimate2(TState state);
    }
}
