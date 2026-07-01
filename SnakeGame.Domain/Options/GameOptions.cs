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