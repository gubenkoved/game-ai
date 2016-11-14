using GameAI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.TicTacToe.UI
{
    public static class TicTacToeUI
    {
        public static void DrawState(TicTacToeState state)
        {
            for (int y = 0; y < state.Size; y++)
            {
                if (y == 0)
                {
                    Console.WriteLine(new string('-', state.Size * 2 + 1));
                }

                for (int x = 0; x < state.Size; x++)
                {
                    if (x == 0)
                    {
                        Console.Write("|");
                    }

                    Player? cellState = state.GetCell(x, y);

                    if (cellState == Player.Maximizing)
                    {
                        Console.Write("x");
                    } else if (cellState == Player.Minimizing)
                    {
                        Console.Write("o");
                    } else
                    {
                        Console.Write(" ");
                    }

                    Console.Write("|");
                }

                Console.WriteLine();
                Console.WriteLine(new string('-', state.Size * 2 + 1));
            }

            Console.WriteLine();
        }
    }
}
