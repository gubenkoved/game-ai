using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.Core.Engines.BruteForce
{
    public class BruteForceAIEngine : AIEngine
    {
        public int MaxDepth { get; set; } = 10;

        public override Move GetBestMove(Game game)
        {
            Move bestMove;
            Estimate bestEstimate;
            GetBestMoveImpl(game, 1, out bestMove, out bestEstimate);

            return bestMove;
        }

        private void GetBestMoveImpl(Game game, int depth, out Move bestMove, out Estimate bestEstimate)
        {
            bool maximaize = game.State.NextMovePlayer == Player.A;

            Move curBestMove = null;
            Estimate curBestEstimate = maximaize ? Estimate.Min : Estimate.Max;

            foreach (Move move in game.GetAllowedMoves())
            {
                game.DoMove(move);

                Estimate estimate = game.State.Estimate;

                if (estimate.IsTerminate || depth == MaxDepth)
                {
                    // no longer go deeper

                    UpdateBestMove(maximaize, move, estimate, ref curBestMove, ref curBestEstimate);
                } else 
                {
                    // go deeper
                    Move bestMoveInternal;
                    Estimate bestEstimateInternal;

                    GetBestMoveImpl(game, depth + 1, out bestMoveInternal, out bestEstimateInternal);

                    UpdateBestMove(maximaize, bestMoveInternal, bestEstimateInternal, ref curBestMove, ref curBestEstimate);
                }

                game.UndoMove(move);
            }

            bestMove = curBestMove;
            bestEstimate = curBestEstimate;

            Trace.WriteLine($"D: {depth}; BM: {bestMove}; E: {bestEstimate}");
        }

        private void UpdateBestMove(bool maximaize,
            Move move, Estimate estimate,
            ref Move bestMove, ref Estimate bestEstimate)
        {
            if (bestMove == null)
            {
                bestMove = move;
                bestEstimate = estimate;
                return;
            }

            if (maximaize)
            {
                if (estimate > bestEstimate)
                {
                    bestEstimate = estimate;
                    bestMove = move;
                }
            }
            else // minimize
            {
                if (estimate < bestEstimate)
                {
                    bestEstimate = estimate;
                    bestMove = move;
                }
            }
        }
    }
}
