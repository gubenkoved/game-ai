using GameAI.Core;
using GameAI.Core.Engines.BruteForce;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GameAI.TicTacToe.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new TicTacToeGame();

            while (!game.State.Estimate.IsTerminate)
            {
                TicTacToeUI.DrawState(game.State);

                if (game.State.NextMovePlayer == Core.Player.A)
                {
                    // let user move
                    Console.Write("MOVE > ");
                    string userMove = Console.ReadLine();

                    Match match = Regex.Match(userMove, "(?<x>[0-9]) (?<y>[0-9])");

                    if (match != null)
                    {
                        int x = int.Parse(match.Groups["x"].Value);
                        int y = int.Parse(match.Groups["y"].Value);

                        var move = new TicTacToeMove()
                        {
                            X = x,
                            Y = y,
                        };

                        game.DoMove(move);
                    } else
                    {
                        Console.WriteLine("Bad move");
                        continue;
                    }
                } else
                {
                    // ui move
                    var ai = new BruteForceAIEngine();

                    EstimatedMove aiMove = ai.GetBestMove(game);

                    Console.WriteLine($"ESTIMATE: {aiMove.Estimate}; BEST MOVE FOR {game.State.NextMovePlayer}: {aiMove.Move}");

                    game.DoMove(aiMove.Move);
                }
            }

            Console.WriteLine("GAME OVER");

            TicTacToeUI.DrawState(game.State);
        }
    }
}
