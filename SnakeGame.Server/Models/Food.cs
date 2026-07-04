using SnakeGame.Domain.Models;

namespace SnakeGame.Models;

/// <summary>
///     Модель бонуса.
/// </summary>
public class Food
{
    /// <summary>
    ///     Символ отображения.
    /// </summary>
    private char _symbol = '%';
    
    /// <summary>
    ///     Высота.
    /// </summary>
    private int _height;
    
    /// <summary>
    ///     Ширина.
    /// </summary>
    private int _width;

    /// <summary>
    ///     Отступ от границы карты, чтобы не учитывать сразу стены.
    /// </summary>
    private const int PADDING_WALLS = 2;
    
    private readonly Random _random = new();

    /// <summary>
    ///     Конструктор.
    /// </summary>
    public Food(int width, int height)
    {
        _width = width - PADDING_WALLS;
        _height = height - PADDING_WALLS;
    }
    
    /// <summary>
    ///     Создать бонус.
    /// </summary>
    /// <param name="excludedPoints"> Позиции где не нужно создавать бонус</param>
    /// <returns> Позицию бонуса. </returns>
    public Point FoodGenerator(IEnumerable<Point> excludedPoints)
    {
        var excluded = excludedPoints.ToHashSet();
        while (true)
        {
            int x = _random.Next(PADDING_WALLS, _width);
            int y = _random.Next(PADDING_WALLS, _height); 
            var point = new Point(x, y, _symbol);
            if (!excluded.Contains(point))
                return point;
        }
        //TODO: Если места для спавна не будет, то будет бесконечный цикл
    }
}