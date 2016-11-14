using GameAI.Core.Engines.AlphaBeta;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.TicTacToe.Test
{
    [TestClass]
    public class AlphaBetaTest : TestBase
    {
        public AlphaBetaTest()
            : base(new AlphaBetaAIEngine())
        {

        }
    }
}
