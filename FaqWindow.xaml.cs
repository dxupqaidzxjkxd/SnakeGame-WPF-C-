using System.Windows;
using SnakeGame.Services;

namespace SnakeGame
{
    /// <summary>Страница «Об игре и разработчиках».</summary>
    public partial class FaqWindow : Window
    {
        private readonly MusicPlayer music = new MusicPlayer();

        public FaqWindow()
        {
            InitializeComponent();
            Loaded += (s, e) => music.Play("faq");
            Closed += (s, e) => music.Dispose();
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            new MenuWindow().Show();
            Close();
        }
    }
}
