using SnakeGame.Domain.Models;

namespace SnakeGame.Models;

/// <summary>
///     Модель змеи.
/// </summary>
public class Snake
{
    private char Symbol { get; } = '@';

    /// <summary>
    ///     Голова змеи.
    /// </summary>
    private Tail? _head;
    
    /// <summary>
    ///     Последний элемент змеи.
    /// </summary>
    private Tail? _tail;
    
    /// <summary>
    ///     Направление змейки.
    /// </summary>
    private Directions _direction = Directions.Right;

    /// <summary>
    ///     Добавление хвоста.
    /// </summary>
    public void AddTail()
    {
        var tail = new Tail();
        if(_head is null)
            _head = tail;
        else
            _tail!.Next = tail;
        _tail = tail;
    }

    /// <summary>
    ///     Установить начальную позицию змейке.
    /// </summary>
    /// <param name="position"></param>
    public void SetStartPosition(Point position)
    {
        var tails = GetTails();
        foreach (var tail in tails)
        {
            tail.Position = position;
        }
    }

    /// <summary>
    ///     Установить направление змейки.
    /// </summary>
    /// <param name="direction"> Направление. </param>
    public void SetDirection(Directions direction)
    {
        if(IsMirrorDirection(direction))
            return;
        _direction = direction;
    }

    /// <summary>
    ///     Сделать шаг змейки.
    /// </summary>
    public void Move()
    {
        var nextPoint = _direction switch
        {
            Directions.Up => new Point(0, -1, Symbol),
            Directions.Down => new Point(0, 1, Symbol),
            Directions.Left => new Point(-1, 0, Symbol),
            Directions.Right => new Point(1, 0, Symbol),
            _ => throw new ArgumentOutOfRangeException()
        };
        var currentPosition = _head!.Position;
        SetNextStep(currentPosition);
        _head.Position = currentPosition += nextPoint;
    }

    /// <summary>
    ///     Получить коллекцию элементов змейки.
    /// </summary>
    /// <returns> Коллекция змейки. </returns>
    public IEnumerable<Tail> GetTails()
    {
        var tails = new List<Tail>();
        var tempTail = _head;
        while (tempTail != null)
        {
            tails.Add(tempTail);
            tempTail = tempTail.Next;
        }

        return tails;
    }

    /// <summary>
    ///     Обновить положение хвоста змейки.
    /// </summary>
    /// <param name="position"></param>
    private void SetNextStep(Point position)
    {
        var tempTail = _head!.Next;
        var tempPos = position;
        while (tempTail != null)
        {
            if (!tempPos.Equals(tempTail.Position))
                (tempTail.Position, tempPos) = (tempPos, tempTail.Position);
            else
                tempPos = tempTail.Position;
            
            tempTail =  tempTail.Next;
        }
    }

    /// <summary>
    ///     Проверить является ли переданное направление зеркальным от текущего.
    /// </summary>
    /// <param name="direction"> Направление. </param>
    /// <returns> Зеркальное ли направление. </returns>
    private bool IsMirrorDirection(Directions direction) => _direction switch
    {
        Directions.Up => direction == Directions.Down,
        Directions.Down => direction == Directions.Up,
        Directions.Right => direction == Directions.Left,
        Directions.Left => direction == Directions.Right,
        _ => throw new ArgumentOutOfRangeException()
    };
}