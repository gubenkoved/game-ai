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

            while (!game.State.IsTerminate)
            {
                TicTacToeUI.DrawState(game.State);

                if (game.State.NextMovePlayer == Core.Player.Maximizing)
                {
                    // let user move
                    Console.Write("MOVE > ");
                    string userMove = Console.ReadLine();

                    IEnumerable<TicTacToeMove> allowedMoves = game.GetAllowedMoves().OfType<TicTacToeMove>();

                    Match match = Regex.Match(userMove, "^(?<x>[0-9]) ?(?<y>[0-9])$");

                    if (match != null && match.Success)
                    {
                        int x = int.Parse(match.Groups["x"].Value);
                        int y = int.Parse(match.Groups["y"].Value);

                        var move = new TicTacToeMove()
                        {
                            X = x,
                            Y = y,
                        };

                        if (allowedMoves.Contains(move))
                        {
                            game.DoMove(move);
                        } else
                        {
                            Console.WriteLine("Move is not allowed");
                            continue;
                        }
                    } else
                    {
                        Console.WriteLine("Unable to understand move -- use \"X Y\" syntax (e.g. 0 0)");
                        continue;
                    }
                } else
                {
                    // ui move
                    var ai = new BruteForceAIEngine();

                    AIResult aiMove = ai.Analyse(game);

                    Console.WriteLine($"ESTIMATE: {aiMove.Estimate}; BEST MOVE FOR {game.State.NextMovePlayer}: {aiMove.BestMove}; MOVES CHKD: {ai.MovesChecked}");

                    game.DoMove(aiMove.BestMove);
                }
            }

            Console.WriteLine("GAME OVER");

            TicTacToeUI.DrawState(game.State);
        }
    }
}
