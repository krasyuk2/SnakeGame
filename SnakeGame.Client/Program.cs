using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using SnakeGame.Client.Models;
using SnakeGame.Domain.Applications;
using SnakeGame.Domain.Models;
using SnakeGame.Domain.Options;
using SnakeGame.Models;

if (OperatingSystem.IsWindows())
{
    Console.SetWindowSize(1, 1); 
    Console.SetBufferSize(80, 50);
    Console.SetWindowSize(60, 40);    
}

var gameOptions = GameOptions.Initialization();
var ipEndPont = IPEndPoint.Parse(gameOptions.Address);

var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
await client.ConnectAsync(ipEndPont);
Draw draw = new Draw();

_ = Task.Run(async () =>
{
    while (true)
    {
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            Directions? direction = keyInfo.Key switch
            {
                ConsoleKey.A or ConsoleKey.LeftArrow => Directions.Left,
                ConsoleKey.D or ConsoleKey.RightArrow => Directions.Right,
                ConsoleKey.W or ConsoleKey.UpArrow => Directions.Up,
                ConsoleKey.S or ConsoleKey.DownArrow => Directions.Down,
                _ => null
            };
            
            if(direction is null)
                continue;
            
            var json = JsonSerializer.Serialize<Directions>(direction.Value);
            await client.SendMessageAsync(Encoding.UTF8.GetBytes(json));
        }
    }
});

while (true)
{
    byte[] data = await client.ReceiveMessageAsync();
    string jsonString = Encoding.UTF8.GetString(data);
    var model = JsonSerializer.Deserialize<GameState>(jsonString);

    var listPointDraw = new List<Point>();
   
    listPointDraw.AddRange(model.PlayerOnePosition);
    listPointDraw.AddRange(model.PlayerTwoPosition);
    listPointDraw.AddRange(model.WallsPosition);
    listPointDraw.Add(model.FoodPosition);
    
    draw.ListDrawPoint(listPointDraw);
}