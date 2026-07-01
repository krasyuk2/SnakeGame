namespace SnakeGame.Domain.Models;

/// <summary>
///     Модель обмена состоянием игры.
/// </summary>
public class GameState
{
    /// <summary>
    ///     Позиция первого игрока.
    /// </summary>
    public IEnumerable<Point> PlayerOnePosition { get; set; } = Enumerable.Empty<Point>();
    
    /// <summary>
    ///     Позиция второго игрока.
    /// </summary>
    public IEnumerable<Point> PlayerTwoPosition { get; set; } = Enumerable.Empty<Point>();
    
    /// <summary>
    ///     Позиция стен.
    /// </summary>
    public IEnumerable<Point> WallsPosition { get; set; } = Enumerable.Empty<Point>();
    
    /// <summary>
    ///     Позиция еды.
    /// </summary>
    public Point FoodPosition { get; set; }
}