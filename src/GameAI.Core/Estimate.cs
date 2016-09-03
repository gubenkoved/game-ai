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

        /// <summary>
        /// The winner player -- if specified, the Game is over.
        /// </summary>
        public Player? Winner;
    }
}
