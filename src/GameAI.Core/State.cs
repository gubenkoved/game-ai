﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.Core
{
    public class State
    {
        public Estimate? Estimate { get; set; }

        public Player NextMovePlayer { get; set; }
    }
}
