using SnakeGame.Domain.Models;

namespace SnakeGame.Models;

/// <summary>
///     Модель змеи.
/// </summary>
public class Snake
{
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
    private Directions _direction;

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
        
    }

    /// <summary>
    ///     Установить направление змейки.
    /// </summary>
    /// <param name="direction"> Направление. </param>
    public void SetDirection(Directions direction)
    {
        
    }

    /// <summary>
    ///     Сделать шаг змейки.
    /// </summary>
    public void Move()
    {
        
    }

    /// <summary>
    ///     Получить коллекцию элементов змейки.
    /// </summary>
    /// <returns> Коллекция змейки. </returns>
    public IEnumerable<Point> GetTails()
    {
        throw new NotImplementedException();
    }
}