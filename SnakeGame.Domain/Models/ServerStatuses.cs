namespace SnakeGame.Domain.Models;

public enum ServerStatuses : int
{
    Started,    // Запустили 
    Created,    // Создали лобби с одним игроком
    Connected,  // Второй игрок подключился 
    Game        // Идет игра
}