using SnakeGame.Domain.Models;

namespace SnakeGame.Models;

/// <summary>
///     Модель змеи.
/// </summary>
public class Snake
{
    /// <summary>
    ///     Символ отображения.
    /// </summary>
    private readonly char _symbol;

    /// <summary>
    ///     Приоритет отрисовки.
    /// </summary>
    private readonly int _priority;
    
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
    private Directions _nextDirection = Directions.Right;
    
    /// <summary>
    ///     Текущее направление змейки.
    /// </summary>
    private Directions _currentDirection = Directions.Right;
    
    /// <summary>
    ///     Головной элемент.
    /// </summary>
    public Tail? Head => _head;

    /// <summary>
    ///     Длина змейки.
    /// </summary>
    private int _tailCount = 0;

    /// <summary>
    ///     Базовый размер змейки.
    /// </summary>
    private readonly int _defaultSize;

    /// <summary>
    ///     Доступ для поля без записи на длину змейки.
    /// </summary>
    public int TailCount
    {
        get => _tailCount;
    }

    public Snake(Point startPosition, char symbol, int priority = 0, int snakeDefaultSize = 5)
    {
        _symbol = symbol;
        _priority = priority;
        _defaultSize = snakeDefaultSize;
        CreateSnakeDefaultSize();     
        SetStartPosition(startPosition);
    }

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
        _tailCount++;
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
        _nextDirection = direction;
    }

    /// <summary>
    ///     Сделать шаг змейки.
    /// </summary>
    public void Move()
    {
        var nextPoint = _nextDirection switch
        {
            Directions.Up => new Point(0, -1, _symbol, _priority),
            Directions.Down => new Point(0, 1, _symbol, _priority),
            Directions.Left => new Point(-1, 0, _symbol, _priority),
            Directions.Right => new Point(1, 0, _symbol, _priority),
            _ => throw new ArgumentOutOfRangeException()
        };
        _currentDirection = _nextDirection;
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
    ///     Получить позиции змейки.
    /// </summary>
    /// <returns> Коллекция позиции змейки. </returns>
    public IEnumerable<Point> GetSnakePoints()
    {
        var points = new List<Point>();
        var tempTail = _head;
        while (tempTail != null)
        {
            points.Add(tempTail.Position);
            tempTail = tempTail.Next;
        }

        return points;
    }

    /// <summary>
    ///     Обрезать змейку до базового значения.
    /// </summary>
    public void ResetSizeSnake()
    {
        var tempTail = _head;
        for (int i = 0; i < _defaultSize - 1; i++)
        {
            tempTail = tempTail!.Next;
        }
        tempTail?.Next = null;
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
    private bool IsMirrorDirection(Directions direction) => _currentDirection switch
    {
        Directions.Up => direction == Directions.Down ,
        Directions.Down => direction == Directions.Up,
        Directions.Right => direction == Directions.Left,
        Directions.Left => direction == Directions.Right,
        _ => throw new ArgumentOutOfRangeException()
    };

    /// <summary>
    ///     Создать змейку default зачения.
    /// </summary>
    private void CreateSnakeDefaultSize()
    {
        for (int i = 0; i < _defaultSize; i++)
        {
            AddTail();
        }
    }
}