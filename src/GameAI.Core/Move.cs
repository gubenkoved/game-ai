using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.Core
{
    /// <summary>
    /// Represents abstract game move. Move transfers one game state to another.
    /// </summary>
    public abstract class Move
    {
        /// <summary>
        /// Gets or sets the priority of move. Higher priority gives a hint to the engine
        /// to consider this move first as more perspective.
        /// </summary>
        public int Priority = 0;
    }
}
