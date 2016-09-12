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
        private struct P
        {
            public int X;
            public int Y;

            public P(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        private P[][] _lines;
        public TicTacToeEstimator(int n)
        {
            // n horizontals + n verticals + 2 diagonals
            _lines = Enumerable.Range(0, n + n + 2)
                .Select(_ => new P[n])
                .ToArray();

            for (int z = 0; z < n; z++)
            {
                // horizontal
                for (int k = 0; k < n; k++)
                {
                    _lines[k][z] = new P(z, k);
                }

                // vertical
                for (int k = 0; k < n; k++)
                {
                    _lines[k + n][z] = new P(k, z);
                }

                // diagonals
                _lines[n + n][z] = new P(z, z);
                _lines[n + n + 1][z] = new P(z, n - z - 1);
            }
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

        private bool IsBoardFull(TicTacToeState state)
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

        private Player? IsLineTaken(TicTacToeState state, P[] line)
        {
            Player? takenByCandidate = null;
            foreach (var p in line)
            {
                Player? cur = state.GetCell(p.X, p.Y);

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
