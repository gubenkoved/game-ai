using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.Core
{
    /// <summary>
    /// Reflects the estimation for the game state.
    /// Positive estimation means favor for Player.A, otherwise Player.B is in better position.
    /// </summary>
    public struct Estimate
    {
        public const int AbsMaxValue = 1000 * 1000;

        public const int AbsInfValue = 1000 * 1000 * 1000;

        #region Data
        public int Value;
        #endregion

        public static readonly Estimate Zero = new Estimate() { Value = 0 };

        public static readonly Estimate Max = new Estimate() { Value = AbsMaxValue };
        public static readonly Estimate Min = new Estimate() { Value = -AbsMaxValue };

        public static readonly Estimate MaxInf = new Estimate() { Value = AbsInfValue };
        public static readonly Estimate MinInf = new Estimate() { Value = -AbsInfValue };

        #region Operators overloading
        public static bool operator >(Estimate left, Estimate right)
        {
            return left.Value > right.Value;
        }

        public static bool operator <(Estimate left, Estimate right)
        {
            return left.Value < right.Value;
        }
        public static bool operator ==(Estimate left, Estimate right)
        {
            return left.Value == right.Value;
        }

        public static bool operator !=(Estimate left, Estimate right)
        {
            return !(left == right);
        }

        public static bool operator >=(Estimate left, Estimate right)
        {
            return left > right || left == right;
        }

        public static bool operator <=(Estimate left, Estimate right)
        {
            return left < right || left == right;
        }
        #endregion

        #region Methods
        public static Estimate FromValue(int value)
        {
            return new Estimate()
            {
                Value = value,
            };
        }

        public static Estimate GetMax(Estimate a, Estimate b)
        {
            return a > b
                ? a
                : b;
        }

        public static Estimate GetMin(Estimate a, Estimate b)
        {
            return a < b
                ? a
                : b;
        }
        #endregion

        public override string ToString()
        {
            return $"<{Value}>";
        }
    }
}
