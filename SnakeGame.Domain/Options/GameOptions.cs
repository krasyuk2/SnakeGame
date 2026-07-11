using System.Text.Json;

namespace SnakeGame.Domain.Options;

/// <summary>
///     Настройки игры.
/// </summary>
public class GameOptions
{
    /// <summary>
    ///     Адрес сервера.
    /// </summary>
    public string Address { get; set; } = "127.0.0.1:7777";

    /// <summary>
    ///     Задержка между игровыми тиками.
    /// </summary>
    public int GameTick { get; set; } = 500;
    
    /// <summary>
    ///     Настройки отображения змеи.
    /// </summary>
    public DisplayElement Snake { get; set; }
    
    /// <summary>
    ///     Настройки отображения стены.
    /// </summary>
    public DisplayElement Wall { get; set; }
    
    /// <summary>
    ///     Настройки отображения бонуса.
    /// </summary>
    public DisplayElement Food { get; set; }
    
    /// <summary>
    ///     Настройки размера карты.
    /// </summary>
    public MapSizeSettings MapSize { get; set; }

    /// <summary>
    ///     Базовый размер змейки;
    /// </summary>
    public int SnakeDefaultSize = 5;

    /// <summary>
    ///     Инициализация настроек.
    /// </summary>
    /// <returns> Настройки игры. </returns>
    public static GameOptions Initialization()
    {
        var json = File.ReadAllText("appsettings.json");
        var root = JsonDocument.Parse(json).RootElement;
        var options = root.GetProperty("GameOptions")
            .Deserialize<GameOptions>();
        if(options == null)
            throw new KeyNotFoundException("GameOptions not found");
        return options;
    }
}