using GameAI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.TicTacToe
{
    public class TicTacToeMove : Move
    {
        public int X;
        public int Y;

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
