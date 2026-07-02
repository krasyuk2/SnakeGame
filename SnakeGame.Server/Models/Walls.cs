

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

    public Walls(int height, int width)
    {
        _height = height;
        _width = width;
    }
    
    /// <summary>
    ///     ВОЗВЕСТИ стены.
    /// </summary>
    public void GenerateWalls()
    {
        _points = new List<Point>();
        for (int i = 0; i < _height; i++)
        {
          
        }
        for (int i = 0; i < _width; i++)
        {
          
        }
    }

    /// <summary>
    ///     Получить позицию стен.
    /// </summary>
    /// <returns> Коллекция позиций стен. </returns>
    public IEnumerable<Point> GetWallsPoint() => _points;
}