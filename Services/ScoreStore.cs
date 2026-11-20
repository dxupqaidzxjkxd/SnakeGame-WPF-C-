using System;
using System.IO;

namespace SnakeGame.Services
{
    /// <summary>
    /// Простое сохранение рекордов: один файл на сложность в %LOCALAPPDATA%\SnakeGame.
    /// </summary>
    public static class ScoreStore
    {
        private static string DirectoryPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "SnakeGame");

        private static string FilePathFor(string difficulty) =>
            Path.Combine(DirectoryPath, $"best-{difficulty}.txt");

        /// <summary>Возвращает лучший результат (0, если ещё нет записи).</summary>
        public static int GetBest(string difficulty)
        {
            try
            {
                string file = FilePathFor(difficulty);
                return File.Exists(file) && int.TryParse(File.ReadAllText(file).Trim(), out int best)
                    ? best
                    : 0;
            }
            catch (IOException)
            {
                return 0;
            }
        }

        /// <summary>Обновляет рекорд, если новый результат лучше. Возвращает true при обновлении.</summary>
        public static bool SubmitScore(string difficulty, int score)
        {
            if (score <= GetBest(difficulty))
            {
                return false;
            }

            try
            {
                Directory.CreateDirectory(DirectoryPath);
                File.WriteAllText(FilePathFor(difficulty), score.ToString());
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }
    }
}
