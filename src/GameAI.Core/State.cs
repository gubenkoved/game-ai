using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.Core
{
    public abstract class State
    {
        public Estimate Estimate;

        public Player NextMovePlayer;
    }
}
