namespace SnakeGame.Domain.Models;

/// <summary>
///     Перечисление типов взаимодействия.
/// </summary>
public enum LobbyActions : int
{
    Create,
    Listen,
    Connection
}