using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using SnakeGame.Domain.Models;
using SnakeGame.Domain.Options;
using SnakeGame.Models;
 
var gameOptions = GameOptions.Initialization();

int height = 40;
int width = 60;

//Создание стен
Walls walls = new Walls(width, height);
walls.GenerateWalls();
var wallsPos = walls.GetWallsPoint();

//Инициализация бонуса
Food food = new Food(width, height);

//Включение сервера
var ipEndPont = IPEndPoint.Parse(gameOptions.Address);
var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
server.Bind(ipEndPont);
server.Listen(2);

//Подключение игроков
var player = await server.AcceptAsync();
Snake snake = new Snake(new Point(5, 5, '@'));
var player2 = await server.AcceptAsync();
Snake snake2 = new Snake(new Point(25, 25, '@'));


_ = Task.Run(async () =>
{
    byte[] readBuffer = new byte[256];
    while (true)
    {
        var answer = await player.ReceiveAsync(readBuffer);
        var directionString = Encoding.UTF8.GetString(readBuffer, 0, answer);
        var direction = JsonSerializer.Deserialize<Directions>(directionString);
        snake.SetDirection(direction);
    }
});

_ = Task.Run(async () =>
{
    byte[] readBuffer = new byte[256];
    while (true)
    {
        var answer = await player2.ReceiveAsync(readBuffer);
        var directionString = Encoding.UTF8.GetString(readBuffer, 0, answer);
        var direction = JsonSerializer.Deserialize<Directions>(directionString);
        snake2.SetDirection(direction);
    }
});

while (true)
{
    await Task.Delay(1000);
    
    
    snake.Move();
    snake2.Move();

    var snakeOnePosition = snake.GetSnakePoints();
    var snakeTwoPosition = snake2.GetSnakePoints();
    var snakeAllPosition = new List<Point>(snakeTwoPosition);
    snakeAllPosition.AddRange(snakeOnePosition);
    
    GameState gameState = new GameState()
    {
        PlayerOnePosition = snakeOnePosition,
        PlayerTwoPosition = snakeTwoPosition,
        WallsPosition = wallsPos,
        FoodPosition = food.FoodGenerator(snakeAllPosition)
    };
    
    var json = JsonSerializer.Serialize(gameState);
    byte[] message = Encoding.UTF8.GetBytes(json);
    
    await player.SendAsync(message);
    await player2.SendAsync(message);
}