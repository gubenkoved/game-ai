using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameAI.TicTacToe;
using GameAI.Core.Engines.BruteForce;
using GameAI.Core;

namespace GameAI.Tests
{
    [TestClass]
    public class TicTacToeTest
    {
        [TestMethod]
        public void CheckFirstMove()
        {
            var game = new TicTacToeGame(new TicTacToeEstimator());

            game.State.SetPlayerMark(Player.A, 1, 1);
            game.State.SetPlayerMark(Player.A, 0, 2);

            var ai = new BruteForceAIEngine();

            var bestMove = ai.GetBestMove(game) as TicTacToeMove;

            Assert.AreEqual(2, bestMove.X);
            Assert.AreEqual(0, bestMove.Y);
        }
    }
}
