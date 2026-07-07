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
    private char _symbol;
    
    /// <summary>
    ///     Приоритет отрисовки.
    /// </summary>
    private readonly int _priority;
    
    /// <summary>
    ///     Высота.
    /// </summary>
    private int _height;
    
    /// <summary>
    ///     Ширина.
    /// </summary>
    private int _width;

    /// <summary>
    ///     Отступ от границы карты, чтобы не учитывать стены.
    /// </summary>
    private const int PADDING_WALLS = 2;
    
    /// <summary>
    ///     Класс рандома.
    /// </summary>
    private readonly Random _random = new();

    /// <summary>
    ///     Текущая позиция бонуса.
    /// </summary>
    private Point _currentPosition;

    /// <summary>
    ///     Конструктор.
    /// </summary>
    public Food(int width, int height, char symbol, int priority  = 0)
    {
        _width = width - PADDING_WALLS;
        _height = height - PADDING_WALLS;
        _symbol = symbol;
        _priority = priority;
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
            var point = new Point(x, y, _symbol, _priority);
            if (!excluded.Contains(point))
            {
                _currentPosition = point;
                return point;
            }
        }
        //TODO: Если места для спавна не будет, то будет бесконечный цикл
    }

    /// <summary>
    ///     Получить текущую позицию бонуса.
    /// </summary>
    /// <returns> Позиция бонуса. </returns>
    public Point GetFoodPoint() => _currentPosition;
}