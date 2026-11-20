using System.Windows;
using SnakeGame.Services;

namespace SnakeGame
{
    /// <summary>Окно победы. Показывает итоговый счёт партии.</summary>
    public partial class WinWindow : Window
    {
        private readonly MusicPlayer music = new MusicPlayer();

        public WinWindow(int score)
        {
            InitializeComponent();
            ScoreRun.Text = $"Ваш результат: {score} очков.";
            Loaded += (s, e) => music.Play("win");
            Closed += (s, e) => music.Dispose();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            new MenuWindow().Show();
            Close();
        }
    }
}
