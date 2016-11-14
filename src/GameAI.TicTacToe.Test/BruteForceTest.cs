using GameAI.Core.Engines.BruteForce;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.TicTacToe.Test
{
    [TestClass]
    public class BruteForceTest : TestBase
    {
        public BruteForceTest()
            :base(new BruteForceAIEngine())
        {

        }
    }
}
