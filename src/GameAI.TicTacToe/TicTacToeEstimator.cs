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
                bool? takenBy = IsLineTaken(state, line);

                if (takenBy != null)
                {
                    return takenBy == true
                        ? Estimate.MaxTerminate
                        : Estimate.MinTerminate;
                }
            }

            if (state.Board.All(x => x != null))
            {
                return Estimate.ZeroTerminate;
            }

            // no lines taken -- we do not care to provide static estimate
            return Estimate.Zero;
        }

        public bool? IsLineTaken(TicTacToeState state, Tuple<int, int>[] line)
        {
            bool? takenByCandidate = null;
            foreach (var position in line)
            {
                bool? cur = state.Board[position.Item1 * 3 + position.Item2];

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
