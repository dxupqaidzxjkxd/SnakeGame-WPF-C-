using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SnakeGame
{
    public partial class faq : Window
    {
        private MediaPlayer player;

        public faq()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new Menu();
            newWindow.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Создание экземпляра MediaPlayer
            player = new MediaPlayer();

            // Получение текущего пути исполняемого файла
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Формирование относительного пути к аудиофайлу
            string audioFilePath = Path.Combine(currentDirectory, "faq.mp3");

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
