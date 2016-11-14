using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.Core.Utils
{
    internal static class EstimateHelper
    {
        public static void AdjustTerminalStateEstimate(int searchDepth, ref Estimate terminateEstimate)
        {
            Debug.Assert(searchDepth >= 0);
            Debug.Assert(terminateEstimate == Estimate.MaxInf
                || terminateEstimate == Estimate.MinInf
                || terminateEstimate == Estimate.Zero);

            if (terminateEstimate == Estimate.Zero)
            {
                return;
            }

            // make AI pick shortest path to win and longest path to lose
            // otherwise there will be funny situation then AI seeing that it won
            // w/o chances for other side will not purse the winning

            // the below table describes with counter-intuitive adjustment value
            // Player\Winning |  MAX    |  MIN    
            // -----------------------------------
            //          MAX   |  -d     |  +d     
            //          MIN   |  -d     |  +d     

            bool maxWin = terminateEstimate == Estimate.MaxInf;

            int sign = maxWin
                ? -1
                : +1;

            int adjustment = sign * searchDepth;

            terminateEstimate.Value += adjustment;
        }
    }
}
