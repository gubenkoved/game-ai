using GameAI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.TicTacToe
{
    public class TicTacToeMove : Move,
        IEquatable<TicTacToeMove>
    {
        public int X;
        public int Y;

        public TicTacToeMove()
        {

        }

        public TicTacToeMove(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TicTacToeMove))
            {
                return false;
            }

            return Equals(obj as TicTacToeMove);
        }

        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }

        public bool Equals(TicTacToeMove other)
        {
            if (other == null)
            {
                return false;
            }

            return X == other.X
                && Y == other.Y;
        }
    }
}
