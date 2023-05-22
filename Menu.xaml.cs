using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace SnakeGame
{

    /// <summary>
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        private MediaPlayer player;
        public Menu()
        {
            InitializeComponent();
        }

        private void OpenDialogWindowButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow dialog = new MainWindow();
            dialog.ShowDialog();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new MainWindow();
            newWindow.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var newWindow = new normal();
            newWindow.Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var newWindow = new hard();
            newWindow.Show();
            this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            var newWindow = new faq();
            newWindow.Show();
            this.Close();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            // Создание экземпляра MediaPlayer
            player = new MediaPlayer();

            // Получение текущего пути исполняемого файла
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;

            // Формирование относительного пути к аудиофайлу
            string audioFilePath = Path.Combine(currentDirectory, "menu.mp3");

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
