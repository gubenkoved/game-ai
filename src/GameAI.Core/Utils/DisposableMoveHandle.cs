using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAI.Core.Utils
{
    internal class DisposableMoveHandle : IDisposable
    {
        private Game _game;
        private Move _move;

        public DisposableMoveHandle(Game game, Move move)
        {
            _game = game;
            _move = move;

            game.DoMove(move);
        }

        public void Dispose()
        {
            _game.UndoMove(_move);
        }

        public static IDisposable New(Game game, Move move)
        {
            return new DisposableMoveHandle(game, move);
        }
    }
}
