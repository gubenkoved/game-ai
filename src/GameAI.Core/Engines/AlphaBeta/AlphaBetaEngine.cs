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
            Estimate estimate = default(Estimate);

            TimeSpan elapsed = Time.It(() =>
            {
                estimate = FindImpl(
                    game: game,
                    alpha: Estimate.MinInf,
                    beta: Estimate.MaxInf,
                    depth: 1,
                    maxDepth: MaxDepth,
                    meta: metadata);
            });

            Debug.Assert(metadata.BestMove != null);

            result.BestMove = metadata.BestMove;
            result.Estimate = estimate;

            result.Metadata.MovesChecked = metadata.MovesChecked;
            result.Metadata.ElapsedSec   = elapsed.TotalSeconds;
            result.Metadata.Rate         = metadata.MovesChecked / elapsed.TotalSeconds;

            return result;
        }

        private Estimate FindImpl(Game game, Estimate alpha, Estimate beta, int depth, int maxDepth, Metadata meta)
        {
            Player player = game.State.NextMovePlayer;
            bool isMax    = player == Player.Maximizing;

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

            Estimate v = isMax
                ? Estimate.MinInf
                : Estimate.MaxInf;

            List<Move> moves = game.GetAllowedMoves()
                .OrderByDescending(x => x.Priority)
                .ToList();

            foreach (Move move in moves)
            {
                meta.MovesChecked += 1;

                using (DisposableMoveHandle.New(game, move))
                {
                    Estimate curEstimate = FindImpl(game, alpha, beta, depth + 1, maxDepth, meta);

                    bool betterMoveFound = isMax
                        ? curEstimate > v
                        : curEstimate < v;

                    if (betterMoveFound)
                    {
                        v = curEstimate;

                        if (depth == 1)
                            meta.BestMove = move;
                    }

                    if (isMax)
                    {
                        alpha = Estimate.Max(alpha, v);
                    }
                    else
                    {
                        beta = Estimate.Min(beta, v);
                    }

#if DEBUG
                    if (depth <= 3)
                    {
                        Trace.WriteLine($"{ new string(' ', depth) } - Move {move} for {player} -- {curEstimate} -- Term? {game.State.IsTerminate}");
                    }
#endif

                    if (alpha >= beta)
                    {
                        // alpha/beta cut-off
                        //Trace.WriteLine($"cut-off on depth {depth}");
                        break;
                    }
                }
            }

            return v;
        }
    }
}
