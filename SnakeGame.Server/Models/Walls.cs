

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
    private char Symbol = '#';
    
    /// <summary>
    ///     Высота.
    /// </summary>
    private int _height;
    
    /// <summary>
    ///     Ширина.
    /// </summary>
    private int _width;
    
    /// <summary>
    ///     Коллекция стен.
    /// </summary>
    private List<Point> _points;

    public Walls(int width, int height)
    {
        _height = height - 1;
        _width = width - 1;
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
                Symbol = Symbol
            };
            var point2 = new Point()
            {
                X = _width,
                Y = i,
                Symbol = Symbol
            };
            _points.AddRange(point1,point2);
        }
        for (int i = 0; i < _width; i++)
        {
            var point1 = new Point()
            {
                X = i,
                Y = 0,
                Symbol = Symbol
            };
            var point2 = new Point()
            {
                X = i,
                Y = _height,
                Symbol = Symbol
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