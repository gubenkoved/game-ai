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
            var game = new TicTacToeGame();

            var ai = new BruteForceAIEngine();

            var bestMove = ai.GetBestMove(game).Move as TicTacToeMove;
        }

        [TestMethod]
        public void CheckMoveToWin()
        {
            var game = new TicTacToeGame();

            game.State.SetCell(1, 1, Player.A);
            game.State.SetCell(2, 0, Player.A);

            var ai = new BruteForceAIEngine();

            var bestMove = ai.GetBestMove(game).Move as TicTacToeMove;

            Assert.AreEqual(0, bestMove.X);
            Assert.AreEqual(2, bestMove.Y);
        }

        [TestMethod]
        public void CheckAfterBadFirstMove()
        {
            var game = new TicTacToeGame();

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

        [TestMethod]
        public void TestCase1()
        {
            // -------
            // |o|x| |
            // -------
            // |o|x|x|
            // -------
            // |x|o| |
            // -------

            var game = new TicTacToeGame();

            game.State.SetCell(0, 0, Player.B);
            game.State.SetCell(1, 0, Player.A);
            game.State.SetCell(2, 0, null);
            game.State.SetCell(0, 1, Player.B);
            game.State.SetCell(1, 1, Player.A);
            game.State.SetCell(2, 1, Player.A);
            game.State.SetCell(0, 2, Player.A);
            game.State.SetCell(1, 2, Player.B);
            game.State.SetCell(2, 2, null);

            game.State.NextMovePlayer = Player.B;

            var ai = new BruteForceAIEngine();

            var bestMove = ai.GetBestMove(game).Move as TicTacToeMove;

            Assert.AreEqual(2, bestMove.X);
            Assert.AreEqual(0, bestMove.Y);
        }

        [TestMethod]
        public void TestCase2()
        {
            // -------
            // |o|x|x|
            // -------
            // |x|o| |
            // -------
            // |o| |x|
            // -------

            var game = new TicTacToeGame();

            game.State.SetCell(0, 0, Player.B);
            game.State.SetCell(1, 0, Player.A);
            game.State.SetCell(2, 0, Player.A);
            game.State.SetCell(0, 1, Player.A);
            game.State.SetCell(1, 1, Player.B);
            game.State.SetCell(2, 1, null);
            game.State.SetCell(0, 2, Player.B);
            game.State.SetCell(1, 2, null);
            game.State.SetCell(2, 2, Player.A);

            game.State.NextMovePlayer = Player.B;

            var ai = new BruteForceAIEngine();

            var bestMove = ai.GetBestMove(game).Move as TicTacToeMove;

            Assert.AreEqual(2, bestMove.X, "x");
            Assert.AreEqual(1, bestMove.Y, "y");
        }


    }
}
