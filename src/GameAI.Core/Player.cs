﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.Core
{
    public enum Player
    {
        /// <summary>
        /// Player that tries to maximize "estimate".
        /// </summary>
        Maximizing = 0,

        /// <summary>
        /// Player that tries to minimize "estimate".
        /// </summary>
        Minimizing = 1,
    }
}
