using GameAI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.TicTacToe
{
    public class TicTacToeEstimator : StateEstimator2<TicTacToeState>
    {
        private Tuple<int, int>[][] _lines;
        public TicTacToeEstimator()
        {
            _lines = new[]
            {
                // horizontal lines
                new [] { Tuple.Create(0, 0), Tuple.Create(1, 0), Tuple.Create(2, 0), },
                new [] { Tuple.Create(0, 1), Tuple.Create(1, 1), Tuple.Create(2, 1), },
                new [] { Tuple.Create(0, 2), Tuple.Create(1, 2), Tuple.Create(2, 2), },

                // vertical
                new [] { Tuple.Create(0, 0), Tuple.Create(0, 1), Tuple.Create(0, 2), },
                new [] { Tuple.Create(1, 0), Tuple.Create(1, 1), Tuple.Create(1, 2), },
                new [] { Tuple.Create(2, 0), Tuple.Create(2, 1), Tuple.Create(2, 2), },

                // digaonals
                new [] { Tuple.Create(0, 0), Tuple.Create(1, 1), Tuple.Create(2, 2), },
                new [] { Tuple.Create(2, 0), Tuple.Create(1, 1), Tuple.Create(0, 2), },
            };
        }

        public override Estimate GetEstimate2(TicTacToeState state)
        {
            foreach (var line in _lines)
            {
                Player? takenBy = IsLineTaken(state, line);

                if (takenBy != null)
                {
                    return takenBy == Player.A
                        ? Estimate.MaxTerminate
                        : Estimate.MinTerminate;
                }
            }

            if (IsBoardFull(state))
            {
                return Estimate.ZeroTerminate;
            }

            // no lines taken -- we do not care to provide static estimate
            return Estimate.Zero;
        }

        public bool IsBoardFull(TicTacToeState state)
        {
            for (int x = 0; x < state.Size; x++)
            {
                for (int y = 0; y < state.Size; y++)
                {
                    if (state.GetCell(x, y) == null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public Player? IsLineTaken(TicTacToeState state, Tuple<int, int>[] line)
        {
            Player? takenByCandidate = null;
            foreach (var position in line)
            {
                Player? cur = state.GetCell(position.Item1, position.Item2);

                if (cur == null)
                {
                    return null;
                }

                if (takenByCandidate == null)
                {
                    takenByCandidate = cur;
                } else
                {
                    if (cur.Value != takenByCandidate.Value)
                    {
                        return null;
                    }
                }
            }

            return takenByCandidate.Value;
        }
    }
}
