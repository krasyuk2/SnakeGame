namespace SnakeGame.Domain.Models;

/// <summary>
///     Модель позиции.
/// </summary>
public struct Point
{
    /// <summary>
    ///     Координата по X.
    /// </summary>
    public int X { get; set; }
    
    /// <summary>
    ///     Координата по Y.
    /// </summary>
    public int Y { get; set; }
    
    /// <summary>
    ///     Символ отображения.
    /// </summary>
    public char Symbol { get; set; }
    
    /// <summary>
    ///     Конструктор.
    /// </summary>
    public Point(int x, int y, char symbol)
    {
        X = x;
        Y = y;
        Symbol = symbol;
    }
}