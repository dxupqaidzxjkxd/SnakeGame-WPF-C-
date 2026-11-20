using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using SnakeGame.Services;

namespace SnakeGame
{
    /// <summary>
    /// Единое игровое окно. Сложность задаётся параметром GameSettings,
    /// поэтому отдельные классы MainWindow/normal/hard больше не нужны.
    /// </summary>
    public partial class GameWindow : Window
    {
        private static readonly Dictionary<GridValue, ImageSource> GridValToImage = new()
        {
            { GridValue.Empty, Images.Empty },
            { GridValue.Snake, Images.Body },
            { GridValue.Food, Images.Food },
        };

        // Кэш трансформаций поворота головы: не создаём RotateTransform каждый кадр.
        private static readonly Dictionary<Direction, Transform> DirToRotation = new()
        {
            { Direction.Up, new RotateTransform(0) },
            { Direction.Right, new RotateTransform(90) },
            { Direction.Down, new RotateTransform(180) },
            { Direction.Left, new RotateTransform(270) },
        };

        private readonly GameSettings settings;
        private readonly Image[,] gridImages;
        private readonly GridValue[,] lastDrawn;
        private readonly MusicPlayer music = new MusicPlayer();

        private GameState gameState;
        private Position oldHeadPos;
        private CancellationTokenSource loopCancellation;
        private bool gameRunning;
        private bool isPaused;

        public GameWindow(GameSettings settings)
        {
            this.settings = settings;
            InitializeComponent();
            Title = $"Змейка — {settings.Name}";
            BestText.Text = $"Рекорд: {ScoreStore.GetBest(settings.Name)}";
            gridImages = SetupGrid();
            lastDrawn = new GridValue[settings.Rows, settings.Cols];
            StartNewGame();
            Loaded += (s, e) =>
            {
                if (!string.IsNullOrEmpty(settings.MusicFile))
                {
                    music.Play(settings.MusicFile);
                }
            };
            Closed += (s, e) =>
            {
                loopCancellation?.Cancel();
                music.Dispose();
            };

            // Стартовое состояние: сразу показываем змейку и еду.
            Draw();
        }

        private void StartNewGame()
        {
            gameState = new GameState(settings.Rows, settings.Cols);
            Array.Clear(lastDrawn, 0, lastDrawn.Length);
            oldHeadPos = default;

            // Сбрасываем визуальное состояние поля перед первой отрисовкой новой партии.
            for (int i = 0; i < settings.Rows; i++)
            {
                for (int j = 0; j < settings.Cols; j++)
                {
                    gridImages[i, j].Source = Images.Empty;
                    gridImages[i, j].RenderTransform = Transform.Identity;
                }
            }
            ScoreText.Text = "Очки: 0";
        }

        #region Игровой цикл

        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // Esc — пауза во время игры.
            if (e.Key == Key.Escape && gameRunning && !gameState.Finished)
            {
                TogglePause();
                e.Handled = true;
                return;
            }

            if (isPaused)
            {
                e.Handled = true;
                return;
            }

            if (Overlay.Visibility == Visibility.Visible && !isPaused)
            {
                e.Handled = true;
                if (!gameRunning)
                {
                    await RunGame();
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState == null || gameState.Finished)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                case Key.A:
                    gameState.ChangeDirection(Direction.Left);
                    break;
                case Key.Right:
                case Key.D:
                    gameState.ChangeDirection(Direction.Right);
                    break;
                case Key.Up:
                case Key.W:
                    gameState.ChangeDirection(Direction.Up);
                    break;
                case Key.Down:
                case Key.S:
                    gameState.ChangeDirection(Direction.Down);
                    break;
            }
        }

        private async Task RunGame()
        {
            gameRunning = true;
            try
            {
                Draw();
                await ShowCountDown();
                Overlay.Visibility = Visibility.Hidden;
                await GameLoop();

                if (gameState.Won || gameState.Score >= settings.WinScore)
                {
                    await ShowWin();
                }
                else
                {
                    await ShowGameOver();
                    StartNewGame();
                }
            }
            finally
            {
                gameRunning = false;
            }
        }

        private async Task GameLoop()
        {
            loopCancellation?.Cancel();
            var cts = new CancellationTokenSource();
            loopCancellation = cts;

            while (!cts.IsCancellationRequested && !gameState.Finished)
            {
                await Task.Delay(settings.TickMs, cts.Token).ContinueWith(_ => { });
                if (cts.IsCancellationRequested)
                {
                    break;
                }

                if (isPaused)
                {
                    continue;
                }

                gameState.Move();
                Draw();

                if (settings.WinScore > 0 && gameState.Score >= settings.WinScore)
                {
                    break;
                }
            }
        }

        private void TogglePause()
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                Overlay.Visibility = Visibility.Visible;
                OverlayText.Text = "Пауза\nНажмите Esc для продолжения";
            }
            else
            {
                Overlay.Visibility = Visibility.Hidden;
            }
        }

        #endregion

        #region Отрисовка

        private Image[,] SetupGrid()
        {
            Image[,] images = new Image[settings.Rows, settings.Cols];
            GameGrid.Rows = settings.Rows;
            GameGrid.Columns = settings.Cols;
            GameGrid.Width = GameGrid.Height * (settings.Cols / (double)settings.Rows);
            for (int i = 0; i < settings.Rows; i++)
            {
                for (int j = 0; j < settings.Cols; j++)
                {
                    Image image = new Image
                    {
                        Source = Images.Empty,
                        RenderTransformOrigin = new Point(0.5, 0.5),
                    };
                    images[i, j] = image;
                    GameGrid.Children.Add(image);
                }
            }
            return images;
        }

        /// <summary>
        /// Инкрементальная отрисовка: обновляем только клетки, значение в которых
        /// изменилось с прошлого хода (для поля 50x50 это ~4 клетки вместо 2500).
        /// </summary>
        private void Draw()
        {
            for (int i = 0; i < settings.Rows; i++)
            {
                for (int j = 0; j < settings.Cols; j++)
                {
                    GridValue gridVal = gameState.Grid[i, j];
                    if (gridVal != lastDrawn[i, j])
                    {
                        gridImages[i, j].Source = GridValToImage[gridVal];
                        gridImages[i, j].RenderTransform = Transform.Identity;
                        lastDrawn[i, j] = gridVal;
                    }
                }
            }

            // Голова всегда перерисовывается (поворот + спрайт Head).
            if (lastDrawn[oldHeadPos.Row, oldHeadPos.Col] == GridValue.Snake)
            {
                gridImages[oldHeadPos.Row, oldHeadPos.Col].Source = Images.Body;
                gridImages[oldHeadPos.Row, oldHeadPos.Col].RenderTransform = Transform.Identity;
            }

            Position headPos = gameState.HeadPosition();
            Image headImage = gridImages[headPos.Row, headPos.Col];
            headImage.Source = Images.Head;
            headImage.RenderTransform = DirToRotation[gameState.Dir];
            oldHeadPos = headPos;

            ScoreText.Text = $"Очки: {gameState.Score}";
        }

        private async Task DrawDeadSnake()
        {
            List<Position> positions = new List<Position>(gameState.SnakePositions());
            for (int i = 0; i < positions.Count; i++)
            {
                Position pos = positions[i];
                gridImages[pos.Row, pos.Col].Source = i == 0 ? Images.DeadHead : Images.DeadBody;
                await Task.Delay(50);
            }
        }

        private async Task ShowCountDown()
        {
            OverlayText.Text = "";
            for (int i = 3; i >= 1; i--)
            {
                OverlayText.Text = i.ToString();
                await Task.Delay(500);
            }
        }

        private async Task ShowGameOver()
        {
            await DrawDeadSnake();
            await Task.Delay(1000);

            bool newRecord = ScoreStore.SubmitScore(settings.Name, gameState.Score);
            if (newRecord)
            {
                BestText.Text = $"Рекорд: {gameState.Score}";
            }

            Overlay.Visibility = Visibility.Visible;
            OverlayText.Text = newRecord
                ? $"Новый рекорд: {gameState.Score}!\nНажмите любую кнопку для старта"
                : "Нажмите любую кнопку для старта";
        }

        private async Task ShowWin()
        {
            await Task.Delay(600); // даём игроку увидеть последний ход
            ScoreStore.SubmitScore(settings.Name, gameState.Score);
            music.Stop();

            var winWindow = new WinWindow(gameState.Score);
            winWindow.Closed += (s, e) => Close();
            winWindow.Show();
        }

        #endregion

        #region Навигация

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            loopCancellation?.Cancel();
            var menu = new MenuWindow();
            menu.Show();
            Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        #endregion
    }
}
