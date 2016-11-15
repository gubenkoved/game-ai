using GameAI.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.TicTacToe
{
    public class TicTacToeEstimator
    {
        [DebuggerDisplay("({X}, {Y})")]
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
        private int _toWin;

        public TicTacToeEstimator(int size, int toWin)
        {
            if (size <= 2)
            {
                throw new ArgumentOutOfRangeException(nameof(size));
            }

            if (toWin <= 2)
            {
                throw new ArgumentOutOfRangeException(nameof(toWin));
            }

            _toWin = toWin;

            // n horizontals + n verticals + 2n diagonals
            List<P[]> lines = new List<P[]>();

            for (int z = 0; z < size; z++)
            {
                // horizontal
                P[] line = new P[size];
                for (int k = 0; k < size; k++)
                {
                    line[k] = new P(z, k);
                }
                lines.Add(line);

                // vertical
                line = new P[size];
                for (int k = 0; k < size; k++)
                {
                    line[k] = new P(k, z);
                }
                lines.Add(line);
            }

            // diagonals
            for (int dx = 0; dx < size; dx++)
            {
                List<P> varLine = new List<P>();
                for (int k = 0; k < size; k++)
                {
                    if (k + dx >= size)
                    {
                        break;
                    }

                    varLine.Add(new P(k + dx, k));
                }
                lines.Add(varLine.ToArray());

                varLine = new List<P>();
                for (int k = 0; k < size; k++)
                {
                    if (size - k - dx - 1 < 0)
                    {
                        break;
                    }

                    varLine.Add(new P(size - k - dx - 1, k));
                }
                lines.Add(varLine.ToArray());
            }

            // diagonals
            for (int dy = 1; dy < size; dy++)
            {
                List<P> varLine = new List<P>();
                for (int k = 0; k < size; k++)
                {
                    if (k + dy >= size)
                    {
                        break;
                    }

                    varLine.Add(new P(k, k + dy));
                }
                lines.Add(varLine.ToArray());

                varLine = new List<P>();
                for (int k = 0; k < size; k++)
                {
                    if (size - k < 0 || k + dy >= size)
                    {
                        break;
                    }

                    varLine.Add(new P(size - k, k + dy));
                }
                lines.Add(varLine.ToArray());
            }

            _lines = lines
                .Where(x => x.Length >= toWin)
                .ToArray();
        }

        public Estimate GetEstimate(TicTacToeState state, out bool isTerminate)
        {
            foreach (var line in _lines)
            {
                Player? takenBy = IsLineTaken(state, line);

                if (takenBy != null)
                {
                    isTerminate = true;

                    return takenBy == Player.Maximizing
                        ? Estimate.MaxInf
                        : Estimate.MinInf;
                }
            }

            if (IsBoardFull(state))
            {
                isTerminate = true;
                return Estimate.Zero;
            }

            // no lines taken -- we do not care to provide static estimate
            isTerminate = false;
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
            // line is taken if there are more than "toWin" consecutive marks

            int counter = 0;
            Player? previous = null;

            foreach (var p in line)
            {
                Player? current = state.GetCell(p.X, p.Y);

                if (current == previous)
                {
                    counter += 1;

                    if (counter >= _toWin && current != null)
                    {
                        return current.Value;
                    }
                }
                else
                {
                    previous = current;
                    counter = 1;
                }
            }

            return null;
        }
    }
}
