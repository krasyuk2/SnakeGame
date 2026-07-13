using System.Net;
using System.Net.Sockets;
using SnakeGame;
using SnakeGame.Applications;
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
server.Listen(2);

var player = await server.AcceptAsync();
var player2 = await server.AcceptAsync();


GameService gameService = new GameService(player, player2, snake, snake2, walls, food, gameOptions);
await gameService.StartGame();