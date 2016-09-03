using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.Core
{
    public abstract class Move
    {
        public abstract State Do(State state);

        public abstract State Undo(State state);
    }
}
