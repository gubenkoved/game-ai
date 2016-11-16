using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.Core
{
    public abstract class Engine
    {
        public abstract EngineResult Analyse(Game game);
    }
}
