using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameAI.Core.Engines.BruteForce;
using GameAI.Core;

namespace GameAI.TicTacToe.Test
{
    public abstract class TestBase
    {
        protected AIEngine _ai;

        public TestBase(AIEngine ai)
        {
            _ai = ai;
        }

        [TestMethod]
        public void CheckFirstMove()
        {
            var game = new TicTacToeGame();

            var bestMove = _ai.GetBestMove(game).Move as TicTacToeMove;
        }

        [TestMethod]
        public void CheckMoveToWin()
        {
            var game = new TicTacToeGame();

            game.State.SetCell(1, 1, Player.Maximizing);
            game.State.SetCell(2, 0, Player.Maximizing);

            var bestMove = _ai.GetBestMove(game).Move as TicTacToeMove;

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

            // bad for Minimizing player move
            game.DoMove(new TicTacToeMove()
            {
                X = 0,
                Y = 1,
            });

            var estimatedMove = _ai.GetBestMove(game);

            Assert.IsTrue(estimatedMove.Estimate.Value > 1000);

            game.DoMove(estimatedMove.Move);
        }

        [TestMethod]
        public void EnsureAIWinsAfterOpponentBadMove()
        {
            var game = new TicTacToeGame();

            game.DoMove(new TicTacToeMove()
            {
                X = 1,
                Y = 1,
            });

            // bad for Minimizing player move
            game.DoMove(new TicTacToeMove()
            {
                X = 0,
                Y = 1,
            });

            // play as AI wants till the end
            while (!game.State.IsTerminate)
            {
                EstimatedMove estimatedMove = _ai.GetBestMove(game);
                game.DoMove(estimatedMove.Move);
            }

            Assert.IsTrue(game.State.IsTerminate);
            Assert.IsTrue(game.State.StaticEstimate >= Estimate.Max);
        }

        [TestMethod]
        public void EnsureDrawGameWhenAIvsAI()
        {
            var game = new TicTacToeGame();

            // play as AI wants till the end
            while (!game.State.IsTerminate)
            {
                EstimatedMove estimatedMove = _ai.GetBestMove(game);
                game.DoMove(estimatedMove.Move);
            }

            Assert.IsTrue(game.State.IsTerminate);
            Assert.IsTrue(game.State.StaticEstimate == Estimate.Zero);
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

            game.State.SetCell(0, 0, Player.Minimizing);
            game.State.SetCell(1, 0, Player.Maximizing);
            game.State.SetCell(2, 0, null);
            game.State.SetCell(0, 1, Player.Minimizing);
            game.State.SetCell(1, 1, Player.Maximizing);
            game.State.SetCell(2, 1, Player.Maximizing);
            game.State.SetCell(0, 2, Player.Maximizing);
            game.State.SetCell(1, 2, Player.Minimizing);
            game.State.SetCell(2, 2, null);

            game.State.NextMovePlayer = Player.Minimizing;

            var bestMove = _ai.GetBestMove(game).Move as TicTacToeMove;

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

            game.State.SetCell(0, 0, Player.Minimizing);
            game.State.SetCell(1, 0, Player.Maximizing);
            game.State.SetCell(2, 0, Player.Maximizing);
            game.State.SetCell(0, 1, Player.Maximizing);
            game.State.SetCell(1, 1, Player.Minimizing);
            game.State.SetCell(2, 1, null);
            game.State.SetCell(0, 2, Player.Minimizing);
            game.State.SetCell(1, 2, null);
            game.State.SetCell(2, 2, Player.Maximizing);

            game.State.NextMovePlayer = Player.Minimizing;

            var bestMove = _ai.GetBestMove(game).Move as TicTacToeMove;

            Assert.AreEqual(2, bestMove.X, "x");
            Assert.AreEqual(1, bestMove.Y, "y");
        }
    }
}
