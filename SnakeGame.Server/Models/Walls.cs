using SnakeGame.Domain.Models;

namespace SnakeGame.Models;

/// <summary>
///     Стены.
/// </summary>
public class Walls
{
    /// <summary>
    ///     Символ отрисовки.
    /// </summary>
    private readonly char _symbol;
    
    /// <summary>
    ///     Приоритет отрисовки.
    /// </summary>
    private readonly int _priority;
    
    /// <summary>
    ///     Высота.
    /// </summary>
    private readonly int _height;
    
    /// <summary>
    ///     Ширина.
    /// </summary>
    private readonly int _width;
    
    /// <summary>
    ///     Коллекция стен.
    /// </summary>
    private List<Point> _points;

    /// <summary>
    ///     Конструктор
    /// </summary>
    public Walls(int width, int height, char symbol, int priority = 0)
    {
        _height = height - 1;
        _width = width - 1;
        _symbol = symbol;
        _priority =  priority;
    }
    
    /// <summary>
    ///     ВОЗВЕСТИ стены.
    /// </summary>
    public void GenerateWalls()
    {
        _points = new List<Point>();
        for (int i = 0; i < _height; i++)
        {
            var point1 = new Point()
            {
                X = 0,
                Y = i,
                Symbol = _symbol,
                DrawPriority = _priority
            };
            var point2 = new Point()
            {
                X = _width,
                Y = i,
                Symbol = _symbol,
                DrawPriority = _priority
            };
            _points.AddRange(point1,point2);
        }
        for (int i = 0; i < _width; i++)
        {
            var point1 = new Point()
            {
                X = i,
                Y = 0,
                Symbol = _symbol,
                DrawPriority = _priority
            };
            var point2 = new Point()
            {
                X = i,
                Y = _height,
                Symbol = _symbol,
                DrawPriority = _priority
            };
            _points.AddRange(point1,point2);
        }
    }

    /// <summary>
    ///     Получить позицию стен.
    /// </summary>
    /// <returns> Коллекция позиций стен. </returns>
    public IEnumerable<Point> GetWallsPoint() => _points;
}