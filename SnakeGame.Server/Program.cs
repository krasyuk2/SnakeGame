using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using SnakeGame.Domain.Applications;
using SnakeGame.Domain.Models;
using SnakeGame.Domain.Options;
using SnakeGame.Models;
 
var gameOptions = GameOptions.Initialization();

int height = 40;
int width = 60;

//Создание стен
var wallSymbol = gameOptions.Wall.Symbol;
var wallDrawPriority = gameOptions.Wall.Priority;
Walls walls = new Walls(width, height, wallSymbol, wallDrawPriority);
walls.GenerateWalls();
var wallsPos = walls.GetWallsPoint();

//Включение сервера
var ipEndPont = IPEndPoint.Parse(gameOptions.Address);
var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
server.Bind(ipEndPont);
server.Listen(2);

//Подключение игроков
var snakeSymbol = gameOptions.Snake.Symbol;
var snakeDrawPriority = gameOptions.Snake.Priority;
var player = await server.AcceptAsync();
Snake snake = new Snake(new Point(5, 5, snakeSymbol, snakeDrawPriority),snakeSymbol, snakeDrawPriority);
var player2 = await server.AcceptAsync();
Snake snake2 = new Snake(new Point(25, 25, snakeSymbol, snakeDrawPriority),snakeSymbol, snakeDrawPriority);

//Инициализация бонуса
var foodSymbol = gameOptions.Food.Symbol;
var foodDrawPriority = gameOptions.Food.Priority;
Food food = new Food(width, height,foodSymbol,foodDrawPriority);
var foodPosition = food.FoodGenerator(GetAllSnakePoints());

_ = Task.Run(async () => await GetDirectionSnake(player2, snake2));
_ = Task.Run(async () => await GetDirectionSnake(player, snake));

while (true)
{
    await Task.Delay(500);
    
    snake.Move();
    snake2.Move();
    
    var isSnakeOneCollision = IsCollision(snake);
    var isSnakeTwoCollision = IsCollision(snake2);
    if(isSnakeOneCollision || isSnakeTwoCollision)
        throw new Exception();
    
    TryEatFood(snake);
    TryEatFood(snake2);
    
    GameState gameState = new GameState()
    {
        PlayerOnePosition =  snake.GetSnakePoints(),
        PlayerTwoPosition = snake2.GetSnakePoints(),
        WallsPosition = wallsPos,
        FoodPosition = foodPosition
    };
    
    var json = JsonSerializer.Serialize(gameState);
    byte[] message = Encoding.UTF8.GetBytes(json);
    
    await player.SendMessageAsync(message);
    await player2.SendMessageAsync(message);
}
async Task GetDirectionSnake(Socket socket, Snake snake)
{
    while (true)
    {
        var data = await socket.ReceiveMessageAsync();
        var direction = JsonSerializer.Deserialize<Directions>(
            Encoding.UTF8.GetString(data));
        snake.SetDirection(direction);
    }
}

void TryEatFood(Snake snake)
{
    var head = snake.Head;
    if (head!.Position.Equals(foodPosition))
    {
        snake.AddTail();
        foodPosition = food.FoodGenerator(GetAllSnakePoints());
    }
}

List<Point> GetAllSnakePoints()
{
    var all = new List<Point>(snake.GetSnakePoints());
    all.AddRange(snake2.GetSnakePoints());
    return all;
}

bool IsCollision(Snake snake)
{
    var collisionList = GetAllSnakePoints();
    var snakeHead = snake.Head!.Position;
    collisionList.Remove(snakeHead);
    collisionList.AddRange(wallsPos);

    if (collisionList.Any(x => x.Equals(snakeHead)))
        return true;
    return false;
}