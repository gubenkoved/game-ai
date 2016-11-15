using GameAI.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.TicTacToe
{
    public class TicTacToeGame
        : Game2<TicTacToeState, TicTacToeMove>
    {
        private TicTacToeEstimator _estimator;

        private int _n;
        private int _toWin;

        #region Presets
        public static TicTacToeGame Classic
        {
            get
            {
                return new TicTacToeGame(size: 3, toWin: 3);
            }
        }
        #endregion

        public TicTacToeGame(int size, int toWin)
        {
            TicTacToeState initState = GetIntitialState(size, toWin);

            Init2(initState);
        }

        public override void Init2(TicTacToeState state)
        {
            _n = state.Size;
            _toWin = state.ToWin;

            _estimator = new TicTacToeEstimator(
                size: _n,
                toWin: _toWin);
            
            State = state;
        }

        public override IEnumerable<Move> GetAllowedMoves()
        {
            for (int x = 0; x < _n; x++)
            {
                for (int y = 0; y < _n; y++)
                {
                    if (State.GetCell(x, y) == null)
                    {
                        yield return new TicTacToeMove()
                        {
                            X = x,
                            Y = y,
                        };
                    }
                }
            }
        }

        protected TicTacToeState GetIntitialState(int size, int toWin)
        {
            return new TicTacToeState(size, toWin)
            {
                StaticEstimate = Estimate.Zero,
                NextMovePlayer = Player.Maximizing,
            };
        }

        protected override void DoMoveImpl2(TicTacToeMove move)
        {
            bool val = State.NextMovePlayer == Player.Maximizing ? true : false;

            State.SetCell(move.X, move.Y, State.NextMovePlayer);
            State.NextMovePlayer = GetOtherPlayer(State.NextMovePlayer);

            ReEstimate();
        }

        protected override void UndoMoveImpl2(TicTacToeMove move)
        {
            State.SetCell(move.X, move.Y, null);
            State.NextMovePlayer = GetOtherPlayer(State.NextMovePlayer);

            ReEstimate();
        }

        private void ReEstimate()
        {
            bool isTerminateState;
            State.StaticEstimate = _estimator.GetEstimate(State, out isTerminateState);
            State.IsTerminate = isTerminateState;
        }
    }
}
