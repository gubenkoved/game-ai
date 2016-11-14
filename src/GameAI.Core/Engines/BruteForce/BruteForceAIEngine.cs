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
        public int MaxDepth = 10;

        public int MovesChecked = 0;

        public override AIResult Analyse(Game game)
        {
            MovesChecked = 0;

            Move bestMove;
            Estimate bestEstimate;
            GetBestMoveImpl(game, 1, out bestMove, out bestEstimate);

            Trace.WriteLine($"Moves checked: {MovesChecked}");

            return new AIResult()
            {
                Estimate = bestEstimate,
                BestMove = bestMove,
            };
        }

        private void GetBestMoveImpl(Game game, int depth, out Move bestMove, out Estimate bestEstimate)
        {
            bool isMaximizeStage = game.State.NextMovePlayer == Player.Maximizing;

            Move curBestMove = null;
            Estimate curBestEstimate = isMaximizeStage ? Estimate.Min : Estimate.Max;

            foreach (Move move in game.GetAllowedMoves())
            {
                MovesChecked += 1;

                game.DoMove(move);

                Estimate estimate = game.State.StaticEstimate;

                // force AI to win faster
                if (game.State.IsTerminate && estimate != Estimate.Zero)
                {
                    estimate.Value += depth * (isMaximizeStage ? -1 : +1);
                }

                if (depth < 3)
                {
                    Trace.WriteLine($"{Tab(depth - 1)}Consider {move}");
                }

                if (game.State.IsTerminate || depth == MaxDepth)
                {
                    // no longer go deeper

                    UpdateBestMove(isMaximizeStage, move, estimate, ref curBestMove, ref curBestEstimate);
                } else 
                {
                    // go deeper
                    Move bestMoveInternal;
                    Estimate bestEstimateInternal;

                    GetBestMoveImpl(game, depth + 1, out bestMoveInternal, out bestEstimateInternal);

                    UpdateBestMove(isMaximizeStage, move, bestEstimateInternal, ref curBestMove, ref curBestEstimate);
                }

                game.UndoMove(move);
            }

            if (curBestMove == null)
            {
                throw new InvalidOperationException("No moves found");
            }

            bestMove = curBestMove;
            bestEstimate = curBestEstimate;

            if (depth <=  3)
                Trace.WriteLine($"{Tab(depth-1)}RET: {depth}; BM: {bestMove}; E: {bestEstimate} ({(isMaximizeStage ? "max" : "min")})");
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

        private string Tab(int n)
        {
            return new string('\t', n);
        }
    }
}
