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
        private readonly TicTacToeEstimator _estimator;

        private readonly int _n;

        public TicTacToeGame(int size = 3)
        {
            _n = size;
            _estimator = new TicTacToeEstimator(size);

            State = GetIntitialState();
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

        protected TicTacToeState GetIntitialState()
        {
            return new TicTacToeState(_n)
            {
                Estimate       = Estimate.Zero,
                NextMovePlayer = Player.A,
            };
        }

        protected override void DoMoveImpl2(TicTacToeMove move)
        {
            bool val = State.NextMovePlayer == Player.A ? true : false;

            State.SetCell(move.X, move.Y, State.NextMovePlayer);
            State.NextMovePlayer = GetOtherPlayer(State.NextMovePlayer);

            State.Estimate = _estimator.GetEstimate2(State);
        }

        protected override void UndoMoveImpl2(TicTacToeMove move)
        {
            State.SetCell(move.X, move.Y, null);
            State.NextMovePlayer = GetOtherPlayer(State.NextMovePlayer);
        }
    }
}
