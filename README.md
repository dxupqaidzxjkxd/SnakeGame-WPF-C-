# Змейка (WPF, C#/.NET 6)

Классическая игра «Змейка» на WPF с тремя уровнями сложности, сохранением рекордов и фоновой музыкой.

## Возможности
- Три сложности (лёгкая/средняя/сложная) — разное поле, скорость и условие победы.
- Единое игровое окно `GameWindow`, настраивается через `GameSettings`.
- Пауза (Esc), управление стрелками или WASD.
- Сохранение рекорда по каждой сложности (`%LOCALAPPDATA%\SnakeGame`).
- Фоновая музыка (необязательная): положите mp3-файлы в папку `Sound` рядом с exe —
  `menu.mp3`, `easy.mp3`, `normal.mp3`, `hard.mp3`, `faq.mp3`, `win.mp3`.

## Структура проекта
- `GameState.cs`, `Direction.cs`, `Position.cs`, `GridValue.cs` — чистая модель игры (без WPF).
- `GameWindow.*` — окно игры для всех сложностей.
- `MenuWindow.*`, `FaqWindow.*`, `WinWindow.*` — меню, «об игре», экран победы.
- `Services/MusicPlayer.cs` — воспроизведение фоновой музыки.
- `Services/ScoreStore.cs` — сохранение рекордов.
- `Assets/Images` — спрайты, фоны, шрифт, иконка.

## Сборка
```
dotnet build -c Release
```
Требуется Windows и .NET 6 SDK (проект WPF).

## Тесты
Логика игры вынесена в `GameState` и покрыта тестами в `SnakeGame.Tests`.
Запуск: `dotnet test`.
