using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.Core
{
    public abstract class AIEngine
    {
        public abstract Move GetBestMove(Game game);
    }
}
