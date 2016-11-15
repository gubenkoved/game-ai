using GameAI.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.TicTacToe.Test
{
    public static class EstimateExtensions
    {
        public static bool IsTerminal(this Estimate e)
        {
            int maxAbsDeviation = 2000;

            if (Math.Abs(e.Value - Estimate.AbsInfValue) > maxAbsDeviation)
            {
                return true;
            }

            return false;
        }

        public static bool SameSignWith(this Estimate e, Estimate other)
        {
            if (e.Value >= 0)
            {
                return other.Value >= 0;
            } else
            {
                return other.Value < 0;
            }
        }

        public static bool IsCloseTo(this Estimate e, Estimate other)
        {
            if (e.IsTerminal())
            {
                if (!other.IsTerminal())
                {
                    return false;
                }

                return e.SameSignWith(other);
            } else // not terminal
            {
                if (other.IsTerminal())
                {
                    return false;
                }

                // both are not terminal there -- allow 10% discrepancy from max abs value

                int maxAbsValue = Math.Abs(Math.Max(e.Value, other.Value));
                int absDiff = Math.Abs(e.Value - other.Value);

                return absDiff < maxAbsValue * 0.1;
            }
        }
    }
}
