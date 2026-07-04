using System.Diagnostics.CodeAnalysis;

namespace SnakeGame.Domain.Models;

/// <summary>
///     Модель позиции.
/// </summary>
public struct Point : IEquatable<Point>
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
    
    /// <summary>
    ///     Оператор сложения координат.
    /// </summary>
    /// <param name="point"> Координата. </param>
    public void operator += (Point point)
    {
        X += point.X;
        Y += point.Y;
    }

    public bool Equals(Point other) => X == other.X && Y == other.Y;

    ///<inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Point p && X == p.X && Y == p.Y;
    
    ///<inheritdoc/>
    public override int GetHashCode() => HashCode.Combine(X, Y);
}