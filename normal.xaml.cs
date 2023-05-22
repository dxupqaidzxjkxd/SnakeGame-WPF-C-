using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.IO;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace SnakeGame
{
    public partial class normal : Window
    {
        private MediaPlayer player;
        private readonly Dictionary<GridValue, ImageSource> gridValToImage = new()
        {
            {GridValue.Empty,Images.Empty },
            {GridValue.Snake,Images.Body },
            {GridValue.Food,Images.Food }

        };

        private readonly Dictionary<Direction, int> dirToRotation = new()
        {
            {Direction.Up, 0},
            {Direction.Right,90 },
            {Direction.Down,180 },
            {Direction.Left,270 }

        };
        private readonly int rows = 35;
        private readonly int cols = 35;
        private bool isWinWindowOpen = false;
        private readonly Image[,] gridImages;
        private GameState gameState;
        private bool gameRunning;
        private bool gamePaused;
        public normal()
        {
            InitializeComponent();
            gridImages = SetupGrid();
            gameState = new GameState(rows, cols);
        }
        private async Task RunGame()
        {
            Draw();
            await ShowCountDown();
            Overlay.Visibility = Visibility.Hidden;
            gamePaused = false;
            await GameLoop();
            await ShowGameOver();
            gameState = new GameState(rows, cols);
        }
        private async void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Overlay.Visibility == Visibility.Visible || gamePaused)
            {
                e.Handled = true;

            }
            if (!gameRunning)
            {
                gameRunning = true;
                await RunGame();
                gameRunning = false;
            }

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gameState.Gameover)
            {
                return;
            }
            switch (e.Key)
            {
                case Key.Left:
                    gameState.ChangeDirection(Direction.Left);
                    break;
                case Key.Right:
                    gameState.ChangeDirection(Direction.Right);
                    break;
                case Key.Up:
                    gameState.ChangeDirection(Direction.Up);
                    break;
                case Key.Down:
                    gameState.ChangeDirection(Direction.Down);
                    break;
            }

        }
        private async Task GameLoop()
        {
            while (!gameState.Gameover)
            {
                await Task.Delay(100);
                gameState.Move();
                Draw();
            }
        }
        private Image[,] SetupGrid()
        {
            Image[,] images = new Image[rows, cols];
            GameGrid.Rows = rows;
            GameGrid.Columns = cols;
            GameGrid.Width = GameGrid.Height * (cols / (double)rows);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    Image image = new Image
                    {
                        Source = Images.Empty,
                        RenderTransformOrigin = new Point(0.5, 0.5)
                    };
                    images[i, j] = image;
                    GameGrid.Children.Add(image);
                }
            }
            return images;
        }
        private void Draw()
        {
            if (gameState.Score >= 20 && !isWinWindowOpen)
            {
                isWinWindowOpen = true;
                ShowWinWindow();
            }
            DrawGrid();
            DrawSnakeHead();
            ScoreText.Text = $"Очки: {gameState.Score}";
        }

        private void ShowWinWindow()
        {
            win win = new win();
            win.Closed += (s, e) => isWinWindowOpen = false;
            win.Show();
            Close();
        }

        private void DrawGrid()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    GridValue gridVal = gameState.Grid[i, j];
                    gridImages[i, j].Source = gridValToImage[gridVal];
                    gridImages[i, j].RenderTransform = Transform.Identity;
                }
            }
        }
        private void DrawSnakeHead()
        {
            Position headPos = gameState.HeadPosition();
            Image image = gridImages[headPos.Row, headPos.Col];
            image.Source = Images.Head;
            int rotation = dirToRotation[gameState.Dir];
            image.RenderTransform = new RotateTransform(rotation);
        }
        private async Task DrawDeadSnake()
        {
            List<Position> positions = new List<Position>(gameState.SnakePositions());
            for (int i = 0; i < positions.Count; i++)
            {
                Position pos = positions[i];
                ImageSource source = (i == 0) ? Images.DeadHead : Images.DeadBody;
                gridImages[pos.Row, pos.Col].Source = source;
                await Task.Delay(50);
            }
        }
        private async Task ShowCountDown()
        {
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
            Overlay.Visibility = Visibility.Visible;
            OverlayText.Text = "Нажмите любую кнопку для старта";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new Menu();
            newWindow.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Создание экземпляра MediaPlayer
            player = new MediaPlayer();

            // Получение текущего пути исполняемого файла
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Формирование относительного пути к аудиофайлу
            string audioFilePath = Path.Combine(currentDirectory, "normal.mp3");

            // Задание пути к аудиофайлу
            player.Open(new Uri(audioFilePath, UriKind.Relative));

            // Обработчик события загрузки окна
            player.MediaOpened += (s, args) =>
            {
                // Воспроизведение звука при открытии окна
                player.Play();
            };

            // Обработчик события ошибки воспроизведения
            player.MediaFailed += (s, args) =>
            {
                MessageBox.Show("Ошибка воспроизведения аудио: " + args.ErrorException.Message);
            };

            // Обработчик события закрытия окна
            Closing += (s, args) =>
            {
                // Остановка воспроизведения при закрытии окна
                player.Stop();
            };
        }
    }
}
