using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using SnakeGame.Applications;
using SnakeGame.Domain.Applications;
using SnakeGame.Domain.Models;
using SnakeGame.Domain.Options;
using SnakeGame.Models;
 
var gameOptions = GameOptions.Initialization();

int height = gameOptions.MapSize.Height;
int width = gameOptions.MapSize.Width;

//Создание стен - у всех одинаковые.
var wallSymbol = gameOptions.Wall.Symbol;
var wallDrawPriority = gameOptions.Wall.Priority;
Walls walls = new Walls(width, height, wallSymbol, wallDrawPriority);
walls.GenerateWalls();
var wallPos = walls.GetWallsPoint();

//Инициализация бонуса - у всех одинаковые.
var foodSymbol = gameOptions.Food.Symbol;
var foodDrawPriority = gameOptions.Food.Priority;
Food food = new Food(width, height,foodSymbol,foodDrawPriority);

//Включение сервера
var ipEndPont = IPEndPoint.Parse(gameOptions.Address);
var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
server.Bind(ipEndPont);
server.Listen(10);

var gameRooms = new List<GameService>();

_ = Task.Run(async () =>
{
    while (true)
    {   
        var player = await server.AcceptAsync(); // и вынести это отдельно и крутить цикл в таске, а то сейчас хуйня
                                                        // он как из switch выйдет нужно будет заново подключатся 
                                                        // и так же accept занят пока не обработается клиент.
        
        var message = await player.ReceiveMessageAsync();
        string jsonString = Encoding.UTF8.GetString(message);
        var lobby = JsonSerializer.Deserialize<Lobby>(jsonString);
        
        switch (lobby!.Action)
        {
            case LobbyActions.Create:
                gameRooms.Add(new GameService(lobby.Name, wallPos, food, gameOptions));
                //Можно было бы создавать тут id и создавать модель lobby, но тогда было бы 2 списка (хочу 1 источник правды)
                //Или ну, можно, конечно, если удаляется игра, то искать ее, и в lobby и там удалять и т.д.
                //Или каждый раз маппить и заполнять список
                break;
            case LobbyActions.Listen: // Нужно сделать так чтобы можно было вызывать много раз
                var list = new List<Lobby>();
                foreach (var game in gameRooms)
                {
                    list.Add(game.GetLobbyInformation());
                }
                
                break;
            case LobbyActions.Connection:
               //Подключение к выбранной комнате по id
                break;
        }
    }
});

Console.ReadLine();

