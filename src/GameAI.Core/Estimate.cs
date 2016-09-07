using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.Core
{
    public struct Estimate
    {
        public int Value;
        public bool IsTerminate;

        public static Estimate Zero
        {
            get { return new Estimate() { Value = 0 }; }
        }

        public static Estimate Max
        {
            get { return new Estimate() { Value = int.MaxValue }; }
        }

        public static Estimate Min
        {
            get { return new Estimate() { Value = int.MinValue }; }
        }

        public static Estimate ZeroTerminate
        {
            get { return new Estimate() { Value = 0, IsTerminate = true, }; }
        }

        public static Estimate MaxTerminate
        {
            get { return new Estimate() { Value = int.MaxValue, IsTerminate = true, }; }
        }

        public static Estimate MinTerminate
        {
            get { return new Estimate() { Value = int.MinValue, IsTerminate = true, }; }
        }

        public static bool operator >(Estimate left, Estimate right)
        {
            return left.Value > right.Value;
        }

        public static bool operator <(Estimate left, Estimate right)
        {
            return left.Value < right.Value;
        }

        public override string ToString()
        {
            string suffix = IsTerminate ? "T" : "";
            return $"[{Value}{suffix}]";
        }
    }
}
