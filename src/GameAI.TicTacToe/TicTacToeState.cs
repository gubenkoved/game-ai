using GameAI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.TicTacToe
{
    public class TicTacToeState : State
    {
        public readonly int Size;
        public readonly int ToWin;

        private readonly bool?[] _board;

        public TicTacToeState(int size, int toWin)
        {
            Size = size;
            ToWin = toWin;
            _board = new bool?[size * size];
        }

        public void SetCell(int x, int y, Player? player)
        {
            bool? val = null;

            if (player != null)
            {
                val = (player == Player.Maximizing ? true : false);
            }

            _board[y * Size + x] = val;
        }

        public Player? GetCell(int x, int y)
        {
            bool? val = _board[y * Size + x];

            if (val == null)
            {
                return null;
            }

            if (val == true)
            {
                return Player.Maximizing;
            }
            else
            {
                return Player.Minimizing;
            }
        }
    }
}
