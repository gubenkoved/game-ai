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
        public bool?[] Board;

        public void SetPlayerMark(Player player, int x, int y)
        {
            Board[x * 3 + y] = player == Player.A
                ? true
                : false;
        }
    }
}
