using GameAI.Core.Engines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.Core
{
    public class EngineResult
    {
        public Move BestMove { get; set; }

        public Estimate Estimate { get; set; }

        public Metadata Metadata { get; set; } = new Metadata();
    }
}
