using System;
using System.Collections.Generic;

namespace SnakeGame
{
    /// <summary>
    /// Чистая модель игры: поле, змейка, еда, счёт. Не знает ничего про WPF,
    /// поэтому её можно тестировать и переиспользовать.
    /// </summary>
    public class GameState
    {
        public int Rows { get; }
        public int Cols { get; }
        public GridValue[,] Grid { get; }
        public Direction Dir { get; private set; }
        public int Score { get; private set; }
        public bool Gameover { get; private set; }

        /// <summary>Победа: набрано нужное число очков либо занято всё поле.</summary>
        public bool Won { get; private set; }

        /// <summary>Партия завершена (проигрыш или победа).</summary>
        public bool Finished => Gameover || Won;

        // Буфер команд поворота (максимум 2, чтобы быстрые нажатия не «съедались»).
        private readonly LinkedList<Direction> dirChanges = new LinkedList<Direction>();

        // Все позиции змейки: First — голова, Last — хвост.
        private readonly LinkedList<Position> snakePositions = new LinkedList<Position>();

        private readonly Random random;

        public GameState(int rows, int cols) : this(rows, cols, new Random())
        {
        }

        /// <summary>Конструктор для юнит-тестов: генератор можно подменить.</summary>
        public GameState(int rows, int cols, Random random)
        {
            if (rows < 3 || cols < 5)
            {
                throw new ArgumentException("Поле слишком маленькое для игры.");
            }

            Rows = rows;
            Cols = cols;
            this.random = random;
            Grid = new GridValue[rows, cols];
            Dir = Direction.Right;
            AddSnake();
            AddFood();
        }

        private void AddSnake()
        {
            // Змейка стартует в центре строки, занимая 3 клетки.
            int startPos = Rows / 2;
            for (int i = 1; i <= 3; i++)
            {
                Grid[startPos, i] = GridValue.Snake;
                snakePositions.AddFirst(new Position(startPos, i));
            }
        }

        private IEnumerable<Position> EmptyPositions()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Cols; j++)
                {
                    if (Grid[i, j] == GridValue.Empty)
                    {
                        yield return new Position(i, j);
                    }
                }
            }
        }

        /// <summary>Ставит еду в случайную свободную клетку. Если свободных нет — это победа.</summary>
        private void AddFood()
        {
            List<Position> emptyPos = new List<Position>(EmptyPositions());
            if (emptyPos.Count == 0)
            {
                Won = true;
                return;
            }

            Position pos = emptyPos[random.Next(emptyPos.Count)];
            Grid[pos.Row, pos.Col] = GridValue.Food;
        }

        public Position HeadPosition() => snakePositions.First.Value;

        public Position TailPosition() => snakePositions.Last.Value;

        public IEnumerable<Position> SnakePositions() => snakePositions;

        private void AddHead(Position pos)
        {
            snakePositions.AddFirst(pos);
            Grid[pos.Row, pos.Col] = GridValue.Snake;
        }

        private void RemoveTail()
        {
            Position tail = snakePositions.Last.Value;
            Grid[tail.Row, tail.Col] = GridValue.Empty;
            snakePositions.RemoveLast();
        }

        private Direction GetLastDirection() =>
            dirChanges.Count == 0 ? Dir : dirChanges.Last.Value;

        private bool CanChangeDirection(Direction newDir)
        {
            if (dirChanges.Count == 2)
            {
                return false;
            }

            Direction lastDir = GetLastDirection();
            return newDir != lastDir && newDir != lastDir.Opposite();
        }

        /// <summary>Команда поворота (помещается в буфер, применяется на следующем ходе).</summary>
        public void ChangeDirection(Direction dir)
        {
            if (CanChangeDirection(dir))
            {
                dirChanges.AddLast(dir);
            }
        }

        private bool OutsideGrid(Position pos) =>
            pos.Row < 0 || pos.Row >= Rows || pos.Col < 0 || pos.Col >= Cols;

        private GridValue WillHit(Position newHeadPos)
        {
            if (OutsideGrid(newHeadPos))
            {
                return GridValue.Outside;
            }

            // Хвост сдвинется в этом же ходе, поэтому в его клетку войти можно.
            if (newHeadPos == TailPosition())
            {
                return GridValue.Empty;
            }

            return Grid[newHeadPos.Row, newHeadPos.Col];
        }

        /// <summary>Сдвигает змейку на один шаг в текущем направлении.</summary>
        public void Move()
        {
            if (Finished)
            {
                return;
            }

            if (dirChanges.Count > 0)
            {
                Dir = dirChanges.First.Value;
                dirChanges.RemoveFirst();
            }

            Position newHeadPos = HeadPosition().Translate(Dir);
            GridValue hit = WillHit(newHeadPos);

            if (hit == GridValue.Outside || hit == GridValue.Snake)
            {
                Gameover = true;
            }
            else if (hit == GridValue.Empty)
            {
                RemoveTail();
                AddHead(newHeadPos);
            }
            else // Food
            {
                AddHead(newHeadPos);
                Score++;
                AddFood();
            }
        }
    }
}
