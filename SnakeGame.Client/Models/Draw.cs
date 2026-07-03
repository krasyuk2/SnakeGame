using SnakeGame.Domain.Models;

namespace SnakeGame.Client.Models;

/// <summary>
///     Отрисовка.
/// </summary>
public class Draw
{
    /// <summary>
    ///     Отрисовка одной позиции.
    /// </summary>
    /// <param name="point"> Позиция. </param>
    public void SingleDrawPoint(Point point)
    {
        Console.SetCursorPosition(point.X, point.Y);
        Console.Write(point.Symbol);
    }
    
    /// <summary>
    ///     Отрисовка нескольких позиций.
    /// </summary>
    /// <param name="points"> Позиции. </param>
    public void ListDrawPoint(IEnumerable<Point> points)
    {
        foreach (var point in points)
        {
            SingleDrawPoint(point);
        }
    }
}