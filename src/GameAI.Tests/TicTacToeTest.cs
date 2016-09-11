using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameAI.TicTacToe;
using GameAI.Core.Engines.BruteForce;
using GameAI.Core;
using System.Diagnostics;

namespace GameAI.Tests
{
    [TestClass]
    public class TicTacToeTest
    {
        [TestMethod]
        public void CheckFirstMove()
        {
            var game = new TicTacToeGame(new TicTacToeEstimator());

            var ai = new BruteForceAIEngine();

            var bestMove = ai.GetBestMove(game).Move as TicTacToeMove;
        }

        [TestMethod]
        public void CheckMoveToWin()
        {
            var game = new TicTacToeGame(new TicTacToeEstimator());

            game.State.SetPlayerMark(Player.A, 1, 1);
            game.State.SetPlayerMark(Player.A, 2, 0);

            var ai = new BruteForceAIEngine();

            var bestMove = ai.GetBestMove(game).Move as TicTacToeMove;

            Assert.AreEqual(0, bestMove.X);
            Assert.AreEqual(2, bestMove.Y);
        }

        [TestMethod]
        public void CheckAfterBadFirstMove()
        {
            var game = new TicTacToeGame(new TicTacToeEstimator());

            game.DoMove(new TicTacToeMove()
            {
                X = 1,
                Y = 1,
            });

            // bad for B move
            game.DoMove(new TicTacToeMove()
            {
                X = 0,
                Y = 1,
            });

            var ai = new BruteForceAIEngine();

            var bestMove = ai.GetBestMove(game);

            Assert.IsTrue(bestMove.Estimate.IsTerminate);
            Assert.IsTrue(bestMove.Estimate.Value > 1000);
        }
    }
}
