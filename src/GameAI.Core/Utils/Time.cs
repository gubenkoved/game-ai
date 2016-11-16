using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.Core.Utils
{
    public class Time
    {
        public static TimeSpan It(Action a)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            a.Invoke();

            sw.Stop();

            return sw.Elapsed;
        }
    }
}
