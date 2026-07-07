using SnakeGame.Domain.Models;

namespace SnakeGame.Client.Models;

/// <summary>
///     Отрисовка.
/// </summary>
public class Draw
{
    /// <summary>
    ///     Коллекция позиций. (те котоыре отрисованы)    
    /// </summary>
    private List<Point> _points = new();
    
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
        var newPoints = points
            .GroupBy(p => (p.X, p.Y))
            .Select(g => g.OrderByDescending(p => p.DrawPriority).First())
            .ToList();

        var drawPoints = newPoints
            .Where(n => !_points.Any(old => old.X == n.X && old.Y == n.Y && old.Symbol == n.Symbol))
            .ToList();
        
        var clearPoint = _points.Except(newPoints).ToList();
        
        ListClear(clearPoint);
        
        foreach (var point in drawPoints)
        {
            SingleDrawPoint(point);
        }
        
        _points = newPoints;
    }

    /// <summary>
    ///     Стереть один символ.
    /// </summary>
    /// <param name="point"> Координаты. </param>
    public void SingleClear(Point point)
    {
        Console.SetCursorPosition(point.X, point.Y);
        Console.Write(" ");
    }

    /// <summary>
    ///     Стереть коллекцию символов.
    /// </summary>
    /// <param name="points"> Коллекция координат. </param>
    public void ListClear(IEnumerable<Point> points)
    {
        foreach (var point in points)
        {
            SingleClear(point);
        }
    }
}