namespace SnakeGame.Domain.Models;

/// <summary>
///     Модель лобби.
/// </summary>
public class Lobby
{
    /// <summary>
    ///     Идентификатор лобби.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    ///     Что хотим сделать.
    /// </summary>
    public LobbyActions Action { get; set; }
    
    /// <summary>
    ///     Название комнаты.
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    ///     Максимальное количесво игроков.
    /// </summary>
    public int? MaxPlayers { get; set; }
}