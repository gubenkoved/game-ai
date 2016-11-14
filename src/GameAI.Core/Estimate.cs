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
        #region Data
        public int Value;
        #endregion

        public bool IsZero
        { get
            {
                return Value == 0;
            }
        }

        public static Estimate Zero
        {
            get { return new Estimate() { Value = 0 }; }
        }

        public static Estimate Max
        {
            get { return new Estimate() { Value = 1000000 }; }
        }

        public static Estimate Min
        {
            get { return new Estimate() { Value = -1000000 }; }
        }

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

        public override string ToString()
        {
            return $"<{Value}>";
        }
    }
}
