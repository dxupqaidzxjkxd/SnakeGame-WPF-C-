#if TESTS
using System;
using SnakeGame;

namespace SnakeGame.Tests
{
    /// <summary>Минимальные проверки модели без внешних фреймворков.</summary>
    internal static class GameStateTests
    {
        private static void Assert(bool condition, string message)
        {
            if (!condition)
            {
                throw new Exception("FAIL: " + message);
            }
        }

        public static void RunAll()
        {
            // Старт: длина 3, счёт 0
            var gs = new GameState(10, 10, new Random(1));
            Assert(!gs.Finished, "игра не должна быть завершена на старте");
            Assert(gs.Score == 0, "начальный счёт 0");

            // Поворот на 180° игнорируется
            gs.ChangeDirection(Direction.Left);
            gs.Move();
            Assert(gs.Dir == Direction.Right, "противоположный поворот запрещён");

            // Выход за границу => проигрыш
            while (!gs.Gameover)
            {
                gs.Move();
            }
            Assert(gs.Gameover && !gs.Won, "столкновение со стеной — проигрыш");

            // После завершения Move() ничего не меняет
            int score = gs.Score;
            gs.Move();
            Assert(gs.Score == score, "ход после окончания партии игнорируется");
        }
    }
}
#endif
