using GameAI.Core.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.Core.Engines.AlphaBeta
{
    public class AlphaBetaEngine : Engine
    {
        private class Metadata
        {
            public Move BestMove;
            public int MovesChecked;
        }

        public int MaxDepth { get; set; } = 10;

        public override EngineResult Analyse(Game game)
        {
            var result = new EngineResult();

            Metadata metadata = new Metadata();

            Estimate estimate = FindImpl(
                game: game,
                alpha: Estimate.MinInf,
                beta: Estimate.MaxInf,
                depth: 1,
                maxDepth: MaxDepth,
                meta: metadata);

            Debug.Assert(metadata.BestMove != null);

            result.BestMove = metadata.BestMove;
            result.Estimate = estimate;
            result.Metadata.MovesChecked = metadata.MovesChecked;

            return result;
        }

        private Estimate FindImpl(Game game, Estimate alpha, Estimate beta, int depth, int maxDepth, Metadata meta)
        {
            Player player = game.State.NextMovePlayer;

            if (maxDepth - depth == 0)
            {
                return game.State.StaticEstimate;
            }

            if (game.State.IsTerminate)
            {
                // make AI pick shortest path to terminate state
                // otherwise there will be funny situation then AI seeing that it won
                // w/o chances for other side will not purse the winning

                Estimate terminateEstimate = game.State.StaticEstimate;

                EstimateHelper.AdjustTerminalStateEstimate(depth, ref terminateEstimate);

                return terminateEstimate;
            }

            if (player == Player.Maximizing)
            {
                // trying to get Alpha higher
                // alpha will be MINIMAL possible outcome of the current position
                // since it's MAXIMIZING player it will be MAX of all the estimates possible out of this position

                Estimate v = Estimate.MinInf;

                foreach (Move move in game.GetAllowedMoves())
                {
                    meta.MovesChecked += 1;

                    using (DisposableMoveHandle.New(game, move))
                    {
                        Estimate curEstimate = FindImpl(game, alpha, beta, depth + 1, maxDepth, meta);

                        if (curEstimate > v)
                        {
                            v = curEstimate;

                            if (depth == 1)
                                meta.BestMove = move;
                        }

                        alpha = Estimate.Max(alpha, v);

#if DEBUG
                        if (depth <= 3)
                        {
                            Trace.WriteLine($"{ new string(' ', depth) } - Move {move} for {player} -- {curEstimate} -- Term? {game.State.IsTerminate}");
                        }
#endif

                        if (alpha >= beta)
                        {
                            // beta cut-off
                            //Trace.WriteLine($"Beta cut-off on depth {depth}");
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
                    meta.MovesChecked += 1;

                    using (DisposableMoveHandle.New(game, move))
                    {
                        Estimate curEstimate = FindImpl(game, alpha, beta, depth + 1, maxDepth, meta);

                        if (curEstimate < v)
                        {
                            v = curEstimate;

                            if (depth == 1)
                                meta.BestMove = move;
                        }

                        beta = Estimate.Min(beta, v);

#if DEBUG
                        if (depth <= 3)
                        {
                            Trace.WriteLine($"{ new string(' ', depth) } - Move {move} for {player} -- {curEstimate} -- Term? {game.State.IsTerminate}");
                        }
#endif

                        if (alpha >= beta)
                        {
                            // alpha cut-off
                            //Trace.WriteLine($"Alpha cut-off on depth {depth}");
                            break;
                        }
                    }
                }

                return v;
            }
        }
    }
}
