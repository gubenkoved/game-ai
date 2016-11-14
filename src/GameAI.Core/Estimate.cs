using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.Core
{
    /// <summary>
    /// Reflects the estimation for the game state.
    /// Positive estimation means favor for Player.Maximizing, otherwise Player.Minimizing is in better position.
    /// </summary>
    public struct Estimate : IEquatable<Estimate>, IComparable<Estimate>
    {
        public const int AbsInfValue = 1000 * 1000 * 1000;

        #region Data
        private int _value;
        public int Value
        {
            get { return _value; }
            set
            {
                Debug.Assert(Math.Abs(value) <= AbsInfValue);

                _value = value;
            }
        }
        #endregion

        public static readonly Estimate Zero = new Estimate() { Value = 0 };

        public static readonly Estimate MaxInf = new Estimate() { Value = AbsInfValue };
        public static readonly Estimate MinInf = new Estimate() { Value = -AbsInfValue };

        #region Interfaces implementation
        public bool Equals(Estimate other)
        {
            return this.CompareTo(other) == 0;
        }

        public int CompareTo(Estimate other)
        {
            if (this.Value == other.Value)
            {
                return 0;
            } else if (this.Value > other.Value)
            {
                return +1;
            } else
            {
                return -1;
            }
        } 
        #endregion

        #region Operators overloading
        public static bool operator >(Estimate left, Estimate right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <(Estimate left, Estimate right)
        {
            return left.CompareTo(right) < 0;
        }
        public static bool operator ==(Estimate left, Estimate right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Estimate left, Estimate right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(null, obj))
            {
                return false;
            }

            if (!(obj is Estimate))
            {
                return false;
            }

            return this.Equals((Estimate)obj);
        }

        public static bool operator >=(Estimate left, Estimate right)
        {
            return left.CompareTo(right) >= 0;
        }

        public static bool operator <=(Estimate left, Estimate right)
        {
            return left.CompareTo(right) <= 0;
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
