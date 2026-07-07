namespace SnakeGame.Domain.Options;

/// <summary>
///     Модель настроек для сущностей.
/// </summary>
public class DisplayElement
{
    /// <summary>
    ///     Символ отображения.
    /// </summary>
    public char Symbol { get; set; }
    
    /// <summary>
    ///     Приоритет отображения.
    /// </summary>
    public int Priority { get; set; }
}