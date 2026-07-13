using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using SnakeGame.Domain.Applications;
using SnakeGame.Domain.Models;
using SnakeGame.Domain.Options;
using SnakeGame.Models;

namespace SnakeGame.Applications;

public class GameService
{
    /// <summary>
    ///     Сокет первого игрока.
    /// </summary>
    private readonly Socket _player1;
    
    /// <summary>
    ///     Сокет второго игрока.
    /// </summary>
    private readonly Socket _player2;
    
    /// <summary>
    ///     Змея первого игрока.
    /// </summary>
    private readonly Snake _snake1;
    
    /// <summary>
    ///     Змея второго игрока.
    /// </summary>
    private readonly Snake _snake2;
    
    /// <summary>
    ///     Стены.
    /// </summary>
    private readonly Walls _walls;
    
    /// <summary>
    ///     Бонусы.
    /// </summary>
    private readonly Food _food;
    
    /// <summary>
    ///     Настройки игры.
    /// </summary>
    private readonly GameOptions _gameOptions;
    
    /// <summary>
    ///     Позиция еды.
    /// </summary>
    private Point _foodPosition;
    
    /// <summary>
    ///     Позиция стен.
    /// </summary>
    private IEnumerable<Point> _wallsPos;

    /// <summary>
    ///     Псевдо рандом.
    /// </summary>
    private Random _random = new();

    /// <summary>
    ///     Конструктор.
    /// </summary>
    public GameService(Socket player1, Socket player2, Snake snake1, Snake snake2, Walls walls, Food food,
        GameOptions gameOptions)
    {
        _player1 = player1;
        _player2 = player2;
        _snake1 = snake1;
        _snake2 = snake2;
        _walls = walls;
        _food = food;
        _gameOptions = gameOptions;
    }

    /// <summary>
    ///     Запустить игру.
    /// </summary>
    public async Task StartGame()
    {
        _foodPosition = _food.FoodGenerator(GetAllSnakePoints());
        _walls.GenerateWalls();
        _wallsPos = _walls.GetWallsPoint();
        
        _ = Task.Run(async () => await GetDirectionSnake(_player1, _snake1));
        _ = Task.Run(async () => await GetDirectionSnake(_player2, _snake2));
        
        while (true)
        {
            await Task.Delay(_gameOptions.GameTick);
    
            _snake1.Move();
            _snake2.Move();
    
            var isSnakeOneCollision = IsCollision(_snake1);
            var isSnakeTwoCollision = IsCollision(_snake2);
            
            if(isSnakeOneCollision)
                ResetSnake(_snake1);
            
            if(isSnakeTwoCollision)
                ResetSnake(_snake2);
            
            if(_snake1.TailCount >= _gameOptions.GameNumberWin)
                Win();
            if(_snake2.TailCount >= _gameOptions.GameNumberWin)
                Win();

            TryEatFood(_snake1);
            TryEatFood(_snake2);
    
            GameState gameState = new GameState()
            {
                PlayerOnePosition =  _snake1.GetSnakePoints(),
                PlayerTwoPosition = _snake2.GetSnakePoints(),
                WallsPosition = _wallsPos,
                FoodPosition = _foodPosition
            };
    
            var json = JsonSerializer.Serialize(gameState);
            byte[] message = Encoding.UTF8.GetBytes(json);
            
            await _player1.SendMessageAsync(message);
            await _player2.SendMessageAsync(message);
        }
    }
    
    /// <summary>
    ///     Считать с клиента направление.
    /// </summary>
    /// <param name="socket"> Сокет клиента. </param>
    /// <param name="snake"> Змея. </param>
    private async Task GetDirectionSnake(Socket socket, Snake snake)
    {
        while (true)
        {
            var data = await socket.ReceiveMessageAsync();
            var direction = JsonSerializer.Deserialize<Directions>(
                Encoding.UTF8.GetString(data));
            snake.SetDirection(direction);
        }
    }

    /// <summary>
    ///     Сбила ли лицом змея бонус.
    /// </summary>
    /// <param name="snake"> Python. </param>
    private bool TryEatFood(Snake snake)
    {
        var head = snake.Head;
        if (head!.Position.Equals(_foodPosition))
        {
            snake.AddTail();
            _foodPosition = _food.FoodGenerator(GetAllSnakePoints());
            return true;
        }
        return false;
    }

    /// <summary>
    ///     Вернуть позиции змей.
    /// </summary>
    /// <returns> Коллекция позиций змей. </returns>
    private List<Point> GetAllSnakePoints()
    {
        var all = new List<Point>(_snake1.GetSnakePoints());
        all.AddRange(_snake2.GetSnakePoints());
        return all;
    }

    /// <summary>
    ///     Проверка коллизии змеи.
    /// </summary>
    /// <param name="snake"> Змея. </param>
    /// <returns> Столкнулась ли с чем то. </returns>
    private bool IsCollision(Snake snake)
    {
        var collisionList = GetAllSnakePoints();
        var snakeHead = snake.Head!.Position;
        collisionList.Remove(snakeHead);
        collisionList.AddRange(_wallsPos);

        if (collisionList.Any(x => x.Equals(snakeHead)))
            return true;
        return false;
    }

    /// <summary>
    ///     Пересоздать змейку со стартовыми значениями.
    /// </summary>
    /// <param name="snake"> Змея. </param>
    private void ResetSnake(Snake snake)
    {
        var newSpawnPoint = GetSpawnPointSnake(snake);
        snake.ResetSizeSnake();
        snake.SetStartPosition(newSpawnPoint);
    }

    /// <summary>
    ///     Получить точку spawn(a) змейки.
    /// </summary>
    /// <param name="snake"> Змея. </param>
    /// <returns> Координата spawn(а). </returns>
    private Point GetSpawnPointSnake(Snake snake)
    {
        var otherSnake = ReferenceEquals(snake, _snake1) ? _snake2 : _snake1;
        var excluded = new HashSet<(int x, int y)>();
        foreach (var wall in _wallsPos) excluded.Add((wall.X, wall.Y));
        foreach (var snakePoint in otherSnake.GetSnakePoints()) excluded.Add((snakePoint.X, snakePoint.Y));

        var spawnList = new List<Point>();
        
        for (int y = 0; y < _gameOptions.MapSize.Height; y++)
        for (int x = 0; x < _gameOptions.MapSize.Width; x++)
        {
            if (IsTrySpawn(x, y, excluded))
                spawnList.Add(new Point(x, y, _gameOptions.Snake.Symbol, _gameOptions.Snake.Priority));
        }
        if(spawnList.Count == 0)
            Win();
        
        return spawnList[_random.Next(spawnList.Count)];
    }

    /// <summary>
    ///     Проверить свободен ли квадрат 3x3 вокруг спавна.
    /// </summary>
    /// <param name="x"> X </param>
    /// <param name="y"> Y </param>
    /// <param name="excluded"> Координаты на которых что-то есть. </param>
    /// <returns> Свободно ли 3x3 вокруг координаты. </returns>
    private bool IsTrySpawn(int x, int y, HashSet<(int x, int y)> excluded)
    {
        for (int dy = -1; dy <= 1; dy++)
        for (int dx = -1; dx <= 1; dx++)
        {
            if(excluded.Contains((dx + x, dy + y)))
                return false;
        }
        return true;
    }

    /// <summary>
    ///     Победа.
    /// </summary>
    private void Win()
    {
        
    }
}