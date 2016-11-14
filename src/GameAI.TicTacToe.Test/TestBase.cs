using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameAI.Core.Engines.BruteForce;
using GameAI.Core;
using System.Diagnostics;

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
            var game = new TicTacToeGame(3);

            AIResult aiResult = _ai.Analyse(game);
            TicTacToeMove bestMove = aiResult.BestMove as TicTacToeMove;

            Assert.IsNotNull(aiResult);
            Assert.IsNotNull(bestMove);

            // check it's one of good first moves -- 4 corners or center
            var goodMoves = new[]
            {
                Tuple.Create(0, 0),
                Tuple.Create(2, 0),
                Tuple.Create(0, 2),
                Tuple.Create(2, 2),
                Tuple.Create(0, 0),
            };

            bool goodMove = false;

            foreach (var p in goodMoves)
            {
                if (bestMove.X == p.Item1 && bestMove.Y == p.Item2)
                {
                    goodMove = true;
                    break;
                }
            }

            Assert.IsTrue(goodMove, $"first move is bad: {bestMove}");
        }

        [TestMethod]
        public void OneMoveToWinTheGame()
        {
            var game = new TicTacToeGame();

            game.State.SetCell(1, 1, Player.Maximizing);
            game.State.SetCell(2, 0, Player.Maximizing);

            game.State.NextMovePlayer = Player.Maximizing;

            AIResult aiResult = _ai.Analyse(game);

            TicTacToeMove bestMove = aiResult.BestMove as TicTacToeMove;

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

            var estimatedMove = _ai.Analyse(game);

            Assert.IsTrue(estimatedMove.Estimate.Value > 1000);

            game.DoMove(estimatedMove.BestMove);
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
                AIResult estimatedMove = _ai.Analyse(game);
                game.DoMove(estimatedMove.BestMove);
            }

            Assert.IsTrue(game.State.IsTerminate);
            Assert.IsTrue(Math.Abs(game.State.StaticEstimate.Value - Estimate.MaxInf.Value) < Estimate.MaxInf.Value / 1000);
        }

        [TestMethod]
        public void EnsureDrawGameWhenAIvsAI()
        {
            var game = new TicTacToeGame();

            int step = 1;

            // play as AI wants till the end
            while (!game.State.IsTerminate )
            {
                Trace.WriteLine($"Step #{step}...");

                AIResult aiResult = _ai.Analyse(game);

                game.DoMove(aiResult.BestMove);

                // draw game estimation all the way till the end
                Assert.IsTrue(aiResult.Estimate == Estimate.Zero, $"step {step} -- draw game expected");

                step += 1;
            }

            Assert.IsTrue(game.State.IsTerminate, "it should be terminate state nows");
            Assert.IsTrue(game.State.StaticEstimate == Estimate.Zero, $"draw game expected");
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

            var bestMove = _ai.Analyse(game).BestMove as TicTacToeMove;

            Assert.AreEqual(2, bestMove.X);
            Assert.AreEqual(0, bestMove.Y);
        }

        [TestMethod]
        public void TestCase2()
        {
            // -------
            // |o|x|x|
            // -------
            // |x|o|*|
            // -------
            // |o| |x|
            // -------
            // * -- expected next move

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

            AIResult aiResult = _ai.Analyse(game);

            Assert.AreEqual(Estimate.Zero, aiResult.Estimate, "it should be draw game estimate");

            var bestMove = aiResult.BestMove as TicTacToeMove;

            Assert.AreEqual(2, bestMove.X, "x");
            Assert.AreEqual(1, bestMove.Y, "y");
        }
    }
}
