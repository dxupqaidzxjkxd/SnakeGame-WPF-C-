using System;
using System.IO;
using System.Windows.Media;

namespace SnakeGame.Services
{
    /// <summary>
    /// Единый сервис фоновой музыки: загружает mp3 из папки Sound рядом с exe,
    /// корректно освобождает ресурсы и не показывает ошибок, если файла нет.
    /// Заменяет дублирующийся код MediaPlayer во всех окнах.
    /// </summary>
    public sealed class MusicPlayer : IDisposable
    {
        private readonly MediaPlayer player = new MediaPlayer();

        /// <summary>Запускает трек Sound/{fileName}.mp3 (если файл существует).</summary>
        public void Play(string fileName)
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sound", fileName + ".mp3");
            if (!File.Exists(path))
            {
                return; // музыка опциональна — не роняем игру молчаливым исключением
            }

            player.Open(new Uri(path, UriKind.Absolute));
            player.Play();
        }

        public void Stop() => player.Stop();

        public void Dispose()
        {
            player.Stop();
            player.Close();
        }
    }
}
