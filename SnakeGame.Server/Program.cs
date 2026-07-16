using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using SnakeGame;
using SnakeGame.Applications;
using SnakeGame.Domain.Applications;
using SnakeGame.Domain.Models;
using SnakeGame.Domain.Options;
using SnakeGame.Models;
 
var gameOptions = GameOptions.Initialization();

int height = gameOptions.MapSize.Height;
int width = gameOptions.MapSize.Width;

//Создание стен
var wallSymbol = gameOptions.Wall.Symbol;
var wallDrawPriority = gameOptions.Wall.Priority;
Walls walls = new Walls(width, height, wallSymbol, wallDrawPriority);

//Воссоздание величественных змей (пока только 2) 
var snakeSymbol = gameOptions.Snake.Symbol;
var snakeDrawPriority = gameOptions.Snake.Priority;
Snake snake = new Snake(new Point(5, 5, snakeSymbol, snakeDrawPriority),snakeSymbol, snakeDrawPriority);
Snake snake2 = new Snake(new Point(25, 25, snakeSymbol, snakeDrawPriority),snakeSymbol, snakeDrawPriority);

//Инициализация бонуса
var foodSymbol = gameOptions.Food.Symbol;
var foodDrawPriority = gameOptions.Food.Priority;
Food food = new Food(width, height,foodSymbol,foodDrawPriority);

//Включение сервера
var ipEndPont = IPEndPoint.Parse(gameOptions.Address);
var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
server.Bind(ipEndPont);
server.Listen(10);

var sockets = new List<Socket>();
var serverStatus =  ServerStatuses.Started;

_ = Task.Run(async () =>
{
    while (true)
    {   
        var player = await server.AcceptAsync();
        var json = JsonSerializer.Serialize<ServerStatuses>(serverStatus);
        await player.SendMessageAsync(Encoding.UTF8.GetBytes(json));
        
        var message = await player.ReceiveMessageAsync();
        string jsonString = Encoding.UTF8.GetString(message);
        var lobby = JsonSerializer.Deserialize<Lobby>(jsonString);
        
        switch (lobby!.Action)
        {
            case LobbyActions.Create:
                //Создаем комнату (GameService) (переделать конструктор)
                break;
            case LobbyActions.Listen:
                //отдаем список комнат (можно где есть места или все)
                break;
            case LobbyActions.Connection:
               //Подключение к выбранной комнате по id
                break;
        }
        
        sockets.Add(player);
    }
});

Console.ReadLine();

/*GameService gameService = new GameService(player, player2, snake, snake2, walls, food, gameOptions);
await gameService.StartGame();*/

