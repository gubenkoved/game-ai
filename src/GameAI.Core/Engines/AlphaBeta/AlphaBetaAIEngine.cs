using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.Core.Engines.AlphaBeta
{
    public class AlphaBetaAIEngine : AIEngine
    {
        public int MaxDepth { get; set; } = 10;

        public override EstimatedMove GetBestMove(Game game)
        {
            throw new NotImplementedException();
        }

        private Estimate FindImpl(Game current, Estimate alpha, Estimate beta, int depthToGo)
        {
            if (depthToGo == 0)
            {
                return current.State.StaticEstimate;
            }

            if (current.State.IsTerminate)
            {
                return current.State.StaticEstimate;
            }

            if (current.State.NextMovePlayer == Player.Maximizing)
            {
                // trying to get Alpha higher
                // alpha will be MINIMAX possible outcome of the current position

                Estimate v = Estimate.Min;
                foreach (Move move in current.GetAllowedMoves())
                {
                    current.DoMove(move);
                    //v = Estimate.GetMax(v, )
                    current.UndoMove(move);
                }
            }

            return Estimate.Zero;
        }
    }
}
