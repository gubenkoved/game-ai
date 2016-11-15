using GameAI.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.TicTacToe.Test
{
    [TestClass]
    public class EstimatorTest
    {
        [TestMethod]
        public void Test5x5()
        {
            var estimator = new TicTacToeEstimator(5, 4);

            var state = new TicTacToeState(5, 4);

            state.SetCell(0, 4, Player.Maximizing);
            state.SetCell(1, 3, Player.Maximizing);
            state.SetCell(2, 2, Player.Maximizing);
            state.SetCell(3, 1, Player.Maximizing);

            bool isTerm;
            var estimate = estimator.GetEstimate(state, out isTerm);

            Assert.IsTrue(isTerm, "it is terminate state");
            Assert.IsTrue(estimate.IsCloseTo(Estimate.MaxInf));
        }
    }
}
