using System;

namespace SnakeGame
{
    /// <summary>Позиция клетки на игровом поле (неизменяемый маленький тип).</summary>
    public readonly struct Position : IEquatable<Position>
    {
        public Position(int row, int col)
        {
            Row = row;
            Col = col;
        }

        /// <summary>Номер строки.</summary>
        public int Row { get; }

        /// <summary>Номер столбца.</summary>
        public int Col { get; }

        /// <summary>Новая позиция, смещённая в заданном направлении.</summary>
        public Position Translate(Direction dir) => new Position(Row + dir.RowOffset, Col + dir.ColOffset);

        public bool Equals(Position other) => Row == other.Row && Col == other.Col;

        public override bool Equals(object obj) => obj is Position other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Row, Col);

        public static bool operator ==(Position left, Position right) => left.Equals(right);

        public static bool operator !=(Position left, Position right) => !left.Equals(right);
    }
}
