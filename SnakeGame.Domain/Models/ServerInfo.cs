namespace SnakeGame.Domain.Models;

/// <summary>
///     Модель для описания сервера.
/// </summary>
public class ServerInfo
{
    /// <summary>
    ///     Адрес сервера.
    /// </summary>
    public string Address { get; set; }
    
    /// <summary>
    ///     Количество игроков на сервере.
    /// </summary>
    public int PlayerCount { get; set; }
    
    /// <summary>
    ///     Максимальное количество пользователей.
    /// </summary>
    public int MaxPlayerCount { get; set; }
    
    /// <summary>
    ///     Наименование сервера.
    /// </summary>
    public string ServerName { get; set; }
}