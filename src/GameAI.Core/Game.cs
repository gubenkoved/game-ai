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
        public virtual State State { get; protected set; }
        #endregion

        public Game()
        {
        }

        #region Public methods
        public abstract IEnumerable<Move> GetAllowedMoves();

        public void DoMove(Move move)
        {
            DoMoveImpl(move);
        }

        public void UndoMove(Move move)
        {
            UndoMoveImpl(move);
        }

        public abstract void Init(State state);
        #endregion

        #region Protected methods
        protected abstract void DoMoveImpl(Move move);
        protected abstract void UndoMoveImpl(Move move);

        protected Player GetOtherPlayer(Player player)
        {
            return player == Player.Maximizing
                ? Player.Minimizing
                : Player.Maximizing;
        }
        #endregion
    }

    public abstract class Game2<TState, TMove> : Game
        where TState : State
        where TMove : Move
    {
        public new TState State
        {
            get
            {
                return base.State as TState;
            }

            protected set
            {
                base.State = value;
            }
        }

        protected sealed override void DoMoveImpl(Move move)
        {
            DoMoveImpl2(move as TMove);
        }

        protected sealed override void UndoMoveImpl(Move move)
        {
            UndoMoveImpl2(move as TMove);
        }

        protected abstract void DoMoveImpl2(TMove move);

        protected abstract void UndoMoveImpl2(TMove move);

        public override void Init(State state)
        {
            Init2(state as TState);
        }

        public abstract void Init2(TState state);
    }
}
