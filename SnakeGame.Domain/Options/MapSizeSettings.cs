namespace SnakeGame.Domain.Options;

/// <summary>
///     Настройки размера карты.
/// </summary>
public class MapSizeSettings
{
    /// <summary>
    ///     Ширина.
    /// </summary>
    public int Width { get; set; }
    
    /// <summary>
    ///     Высота.
    /// </summary>
    public int Height { get; set; }
}