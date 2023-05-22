using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.IO;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace SnakeGame
{
    /// <summary>
    /// Логика взаимодействия для win.xaml
    /// </summary>
    public partial class win : Window
    {
        private MediaPlayer player;
        private bool isWinWindowOpen = false;
        public win()
        {
            InitializeComponent();
            Closed += (s, e) => isWinWindowOpen = false;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Menu win = new Menu();
            win.Closed += (s, e) => isWinWindowOpen = false;
            win.Show();
            this.Close();
        }
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            // Создание экземпляра MediaPlayer
            player = new MediaPlayer();

            // Получение текущего пути исполняемого файла
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Формирование относительного пути к аудиофайлу
            string audioFilePath = Path.Combine(currentDirectory, "win.mp3");

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
