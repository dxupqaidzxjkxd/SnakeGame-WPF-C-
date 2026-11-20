using System.Windows;
using SnakeGame.Services;

namespace SnakeGame
{
    /// <summary>Главное меню: выбор сложности, FAQ, выход.</summary>
    public partial class MenuWindow : Window
    {
        private readonly MusicPlayer music = new MusicPlayer();

        public MenuWindow()
        {
            InitializeComponent();
            BestEasyText.Text = $"EASY: {ScoreStore.GetBest(GameSettings.Easy.Name)}";
            BestNormalText.Text = $"NORMAL: {ScoreStore.GetBest(GameSettings.Normal.Name)}";
            BestHardText.Text = $"HARD: {ScoreStore.GetBest(GameSettings.Hard.Name)}";

            Loaded += (s, e) => music.Play("menu");
            Closed += (s, e) => music.Dispose();
        }

        private void OpenGame(GameSettings settings)
        {
            new GameWindow(settings).Show();
            Close();
        }

        private void EasyButton_Click(object sender, RoutedEventArgs e) => OpenGame(GameSettings.Easy);

        private void NormalButton_Click(object sender, RoutedEventArgs e) => OpenGame(GameSettings.Normal);

        private void HardButton_Click(object sender, RoutedEventArgs e) => OpenGame(GameSettings.Hard);

        private void FaqButton_Click(object sender, RoutedEventArgs e)
        {
            new FaqWindow().Show();
            Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
    }
}
