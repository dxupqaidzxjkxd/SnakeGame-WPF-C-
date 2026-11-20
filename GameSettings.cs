namespace SnakeGame
{
    /// <summary>
    /// Настройки одной партии: размер поля, скорость и условие победы.
    /// Заменяют три почти идентичных окна (MainWindow/normal/hard).
    /// </summary>
    public sealed class GameSettings
    {
        public GameSettings(string name, int rows, int cols, int tickMs, int winScore, string? musicFile)
        {
            Name = name;
            Rows = rows;
            Cols = cols;
            TickMs = tickMs;
            WinScore = winScore;
            MusicFile = musicFile;
        }

        /// <summary>Отображаемое название сложности.</summary>
        public string Name { get; }

        /// <summary>Высота поля в клетках.</summary>
        public int Rows { get; }

        /// <summary>Ширина поля в клетках.</summary>
        public int Cols { get; }

        /// <summary>Задержка между ходами змейки, мс.</summary>
        public int TickMs { get; }

        /// <summary>Очки, необходимые для победы (0 — победа только при заполнении поля).</summary>
        public int WinScore { get; }

        /// <summary>Файл фоновой музыки в Assets/Sounds (без расширения).</summary>
        public string? MusicFile { get; }

        public static GameSettings Easy { get; } =
            new("Лёгкая", rows: 20, cols: 20, tickMs: 150, winScore: 10, musicFile: "easy");

        public static GameSettings Normal { get; } =
            new("Средняя", rows: 35, cols: 35, tickMs: 100, winScore: 20, musicFile: "normal");

        public static GameSettings Hard { get; } =
            new("Сложная", rows: 50, cols: 50, tickMs: 40, winScore: 30, musicFile: "hard");
    }
}
