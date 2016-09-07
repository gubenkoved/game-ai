﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.Core
{
    public abstract class Game
    {
        #region Properties
        public virtual StateEstimator Estimator { get; set; }

        public virtual State State { get; set; }
        #endregion

        public Game()
        {
            State = GetIntitialState();
        }

        #region Public methods
        public abstract IEnumerable<Move> GetAllowedMoves();

        public void DoMove(Move move)
        {
            DoMoveImpl(move);

            Estimate nextStateEstimate = Estimator.GetEstimate(State);

            State.Estimate = nextStateEstimate;
        }

        public void UndoMove(Move move)
        {
            UndoMoveImpl(move);

            Estimate nextStateEstimate = Estimator.GetEstimate(State);

            State.Estimate = nextStateEstimate;
        }
        #endregion

        #region Protected methods
        protected abstract State GetIntitialState();

        protected abstract void DoMoveImpl(Move move);
        protected abstract void UndoMoveImpl(Move move);

        protected Player GetOtherPlayer(Player player)
        {
            return player == Player.A
                ? Player.B
                : Player.A;
        }
        #endregion
    }

    public abstract class Game2<TState, TMove, TEstimator> : Game
        where TState : State
        where TMove : Move
        where TEstimator : StateEstimator2<TState>
    {
        public Game2(TEstimator estimator)
        {
            base.Estimator = estimator;
        }

        public new TEstimator Estimator
        {
            get
            {
                return base.Estimator as TEstimator;
            }
        }

        public new TState State
        {
            get
            {
                return base.State as TState;
            }

            set
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

        public abstract void DoMoveImpl2(TMove move);

        public abstract void UndoMoveImpl2(TMove move);
    }
}