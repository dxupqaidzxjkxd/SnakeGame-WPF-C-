using System;

namespace SnakeGame
{
    /// <summary>Направление движения змейки (неизменяемый маленький тип).</summary>
    public readonly struct Direction : IEquatable<Direction>
    {
        public static readonly Direction Left = new Direction(0, -1);
        public static readonly Direction Right = new Direction(0, 1);
        public static readonly Direction Up = new Direction(-1, 0);
        public static readonly Direction Down = new Direction(1, 0);

        /// <summary>Смещение по строкам.</summary>
        public int RowOffset { get; }

        /// <summary>Смещение по столбцам.</summary>
        public int ColOffset { get; }

        private Direction(int rowOffset, int colOffset)
        {
            RowOffset = rowOffset;
            ColOffset = colOffset;
        }

        /// <summary>Противоположное направление.</summary>
        public Direction Opposite() => new Direction(-RowOffset, -ColOffset);

        public bool Equals(Direction other) =>
            RowOffset == other.RowOffset && ColOffset == other.ColOffset;

        public override bool Equals(object obj) => obj is Direction other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(RowOffset, ColOffset);

        public static bool operator ==(Direction left, Direction right) => left.Equals(right);

        public static bool operator !=(Direction left, Direction right) => !left.Equals(right);
    }
}
