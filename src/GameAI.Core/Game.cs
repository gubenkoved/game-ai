using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.Core
{
    public abstract class Game
    {
        #region Properties
        public StateEstimator Estimator { get; set; }

        public State State { get; set; }
        #endregion

        public Game()
        {
            State = GetIntitialState();
        }

        #region Public methods
        public abstract IEnumerable<Move> GetAllowedMoves();

        public void DoMove(Move move)
        {
            State nextState = move.Do(State);

            Estimate nextStateEstimate = Estimator.GetEstimate(nextState);

            nextState.Estimate = nextStateEstimate;

            State = nextState;
        }

        public void UndoMove(Move move)
        {
            State nextState = move.Undo(State);

            Estimate nextStateEstimate = Estimator.GetEstimate(nextState);

            nextState.Estimate = nextStateEstimate;

            State = nextState;
        }
        #endregion

        #region Protected methods
        protected abstract State GetIntitialState(); 
        #endregion
    }
}
