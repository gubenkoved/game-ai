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
        private TicTacToeEstimator Estimator = new TicTacToeEstimator();

        public const int Size = 3;

        public TicTacToeGame()
        {
        }

        public override IEnumerable<Move> GetAllowedMoves()
        {
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
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

        protected override State GetIntitialState()
        {
            return new TicTacToeState(Size)
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

            State.Estimate = Estimator.GetEstimate2(State);
        }

        protected override void UndoMoveImpl2(TicTacToeMove move)
        {
            State.SetCell(move.X, move.Y, null);
            State.NextMovePlayer = GetOtherPlayer(State.NextMovePlayer);
        }
    }
}
