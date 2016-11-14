using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.Core
{
    public abstract class State
    {
        /// <summary>
        /// Gets or sets "static" state estimate.
        /// This estimate just a simple static metric to serve as a builder block for iterative algorithms.
        /// The game with all the game rules in charge or providing such estimate.
        /// </summary>
        public virtual Estimate StaticEstimate { get; set; }

        /// <summary>
        /// Gets or sets the flag that shows whether position is terminate or not.
        /// </summary>
        public virtual bool IsTerminate { get; set; }

        /// <summary>
        /// Gets or sets the player who should turn next in this state.
        /// </summary>
        public virtual Player NextMovePlayer { get; set; }
    }
}
