using GameAI.Core.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.Core.Engines.AlphaBeta
{
    public class AlphaBetaAIEngine : AIEngine
    {
        public int MaxDepth { get; set; } = 10;

        public override AIResult Analyse(Game game)
        {
            Move move = null;

            Estimate estimate = FindImpl(
                game: game,
                alpha: Estimate.Min,
                beta: Estimate.Max,
                depth: 0,
                maxDepth: MaxDepth,
                bestMove: ref move);

            Debug.Assert(move != null);

            return new AIResult()
            {
                BestMove = move,
                Estimate = estimate,
            };
        }

        private Estimate FindImpl(Game game, Estimate alpha, Estimate beta, int depth, int maxDepth, ref Move bestMove)
        {
            if (maxDepth - depth == 0)
            {
                return game.State.StaticEstimate;
            }

            if (game.State.IsTerminate)
            {
                return game.State.StaticEstimate;
            }

            if (game.State.NextMovePlayer == Player.Maximizing)
            {
                // trying to get Alpha higher
                // alpha will be MINIMAL possible outcome of the current position
                // since it's MAXIMIZING player it will be MAX of all the estimates possible out of this position

                Estimate v = Estimate.MinInf;

                foreach (Move move in game.GetAllowedMoves())
                {
                    using (DisposableMoveHandle.New(game, move))
                    {
                        Estimate estimate = FindImpl(game, alpha, beta, depth + 1, maxDepth, ref bestMove);

                        alpha = Estimate.GetMax(alpha, v);

                        if (estimate > v)
                        {
                            v = estimate;

                            if (depth == 0)
                                bestMove = move;
                        }

                        if (alpha >= beta)
                        {
                            // beta cut-off
                            Trace.WriteLine($"Beta cut-off on depth {depth}");
                            break;
                        }
                    }
                }

                return v;
            } else // MINIMIZING player
            {
                // the same as above rewritten for MIN player
                // beta starts from +INF and gets lower

                Estimate v = Estimate.MaxInf;

                foreach (Move move in game.GetAllowedMoves())
                {
                    using (DisposableMoveHandle.New(game, move))
                    {
                        Estimate estimate = FindImpl(game, alpha, beta, depth + 1, maxDepth, ref bestMove);

                        beta = Estimate.GetMin(alpha, v);

                        if (estimate < v)
                        {
                            v = estimate;

                            if (depth == 0)
                                bestMove = move;
                        }

                        if (alpha >= beta)
                        {
                            // alpha cut-off
                            Trace.WriteLine($"Alpha cut-off on depth {depth}");
                            break;
                        }
                    }
                }

                return v;
            }
        }
    }
}
