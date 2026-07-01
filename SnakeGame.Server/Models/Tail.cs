using SnakeGame.Domain.Models;

namespace SnakeGame.Models;

/// <summary>
///     Хвост змеи. (суть односвязный список)
/// </summary>
public class Tail
{
    /// <summary>
    ///     Позиция.
    /// </summary>
    public Point Position { get; set; }
    
    /// <summary>
    ///     Следующий элемент.
    /// </summary>
    public Tail? Next { get; set; }
}