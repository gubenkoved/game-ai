using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameAI.Core.Engines.BruteForce;
using GameAI.Core;
using System.Diagnostics;

namespace GameAI.TicTacToe.Test
{
    public abstract class TestBase
    {
        protected Engine _ai;


        public TestBase(Engine ai)
        {
            _ai = ai;
        }

        [TestMethod]
        public void CheckFirstMove()
        {
            var game = TicTacToeGame.Classic;

            EngineResult aiResult = _ai.Analyse(game);
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
            var game = TicTacToeGame.Classic;

            game.State.SetCell(1, 1, Player.Maximizing);
            game.State.SetCell(2, 0, Player.Maximizing);

            game.State.NextMovePlayer = Player.Maximizing;

            EngineResult aiResult = _ai.Analyse(game);

            TicTacToeMove bestMove = aiResult.BestMove as TicTacToeMove;

            Assert.AreEqual(0, bestMove.X);
            Assert.AreEqual(2, bestMove.Y);
        }

        [TestMethod]
        public void CheckAfterBadFirstMove()
        {
            var game = TicTacToeGame.Classic;

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
        public void EnsureMaximizingPlayerWinsAfterOpponentBadMove()
        {
            var game = TicTacToeGame.Classic;

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

            int step = 1;

            // play as AI wants till the end
            while (!game.State.IsTerminate)
            {
                EngineResult aiResult = _ai.Analyse(game);

                game.DoMove(aiResult.BestMove);

                // Maximizing wins estimation all the way till the end
                Assert.IsTrue(aiResult.Estimate.IsCloseTo(Estimate.MaxInf), $"step {step} -- win MAX estimation expected");

                step += 1;
            }

            Assert.IsTrue(game.State.IsTerminate);
            Assert.IsTrue(game.State.StaticEstimate.IsCloseTo(Estimate.MaxInf));
        }

        [TestMethod]
        public void EnsureMinimizingPlayerWinsAfterOpponentBadMove()
        {
            var game = TicTacToeGame.Classic;

            game.DoMove(new TicTacToeMove(0, 1));
            game.DoMove(new TicTacToeMove(1, 1));

            // bad move
            game.DoMove(new TicTacToeMove(2, 1));

            int step = 1;

            // play as AI wants till the end
            while (!game.State.IsTerminate)
            {
                EngineResult aiResult = _ai.Analyse(game);

                game.DoMove(aiResult.BestMove);

                // Minimizing wins estimation all the way till the end
                Assert.IsTrue(aiResult.Estimate.IsCloseTo(Estimate.MinInf), $"step {step} -- win for MIN estimation expected");

                step += 1;
            }

            Assert.IsTrue(game.State.IsTerminate);
            Assert.IsTrue(game.State.StaticEstimate.IsCloseTo(Estimate.MinInf));
        }

        [TestMethod]
        public void EnsureDrawGameWhenAIvsAI()
        {
            var game = TicTacToeGame.Classic;

            int step = 1;

            // play as AI wants till the end
            while (!game.State.IsTerminate )
            {
                Trace.WriteLine($"Step #{step}...");

                EngineResult aiResult = _ai.Analyse(game);

                game.DoMove(aiResult.BestMove);

                // draw game estimation all the way till the end
                Assert.IsTrue(aiResult.Estimate.IsCloseTo(Estimate.Zero), $"step {step} -- draw game expected");

                step += 1;
            }

            Assert.IsTrue(game.State.IsTerminate, "it should be terminate state now");
            Assert.IsTrue(game.State.StaticEstimate.IsCloseTo(Estimate.Zero), $"draw game expected");
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

            var game = TicTacToeGame.Classic;

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

            var game = TicTacToeGame.Classic;

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

            EngineResult aiResult = _ai.Analyse(game);

            Assert.AreEqual(Estimate.Zero, aiResult.Estimate, "it should be draw game estimate");

            var bestMove = aiResult.BestMove as TicTacToeMove;

            Assert.AreEqual(2, bestMove.X, "x");
            Assert.AreEqual(1, bestMove.Y, "y");
        }
    }
}
