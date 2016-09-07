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
        : Game2<TicTacToeState, TicTacToeMove, TicTacToeEstimator>
    {
        public const int Size = 3;

        public TicTacToeGame(TicTacToeEstimator estimator)
            : base(estimator)
        {
        }

        public override IEnumerable<Move> GetAllowedMoves()
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    if (State.Board[i * Size + j] == null)
                    {
                        yield return new TicTacToeMove()
                        {
                            X = i,
                            Y = j,
                        };
                    }
                }
            }
        }

        protected override State GetIntitialState()
        {
            return new TicTacToeState()
            {
                Board          = new bool?[Size * Size],
                Estimate       = Estimate.Zero,
                NextMovePlayer = Player.A,
            };
        }

        public override void DoMoveImpl2(TicTacToeMove move)
        {
            Contract.Requires(State.Board[move.X * Size + move.Y] == null);
            Contract.Ensures(State.Board[move.X * Size + move.Y] != null);

            bool val = State.NextMovePlayer == Player.A ? true : false;

            State.Board[move.X * Size + move.Y] = val;
            State.NextMovePlayer = GetOtherPlayer(State.NextMovePlayer);
        }

        public override void UndoMoveImpl2(TicTacToeMove move)
        {
            Contract.Requires(State.Board[move.X * Size + move.Y] != null);
            Contract.Ensures(State.Board[move.X * Size + move.Y] == null);

            State.Board[move.X * Size + move.Y] = null;
        }
    }
}
