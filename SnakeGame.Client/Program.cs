using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using SnakeGame.Client.Models;
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
byte[] readBuffer = new byte[10240];

_ = Task.Run(async () =>
{
    while (true)
    {
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            var direction = keyInfo.Key switch
            {
                ConsoleKey.A or ConsoleKey.LeftArrow => Directions.Left,
                ConsoleKey.D or ConsoleKey.RightArrow => Directions.Right,
                ConsoleKey.W or ConsoleKey.UpArrow => Directions.Up,
                ConsoleKey.S or ConsoleKey.DownArrow => Directions.Down
            };
            var json = JsonSerializer.Serialize<Directions>(direction);
            await client.SendAsync(Encoding.UTF8.GetBytes(json));
        }
    }
});

while (true)
{
    Array.Clear(readBuffer, 0, readBuffer.Length);
    int bytesRead = await client.ReceiveAsync(readBuffer);
    string jsonString = Encoding.UTF8.GetString(readBuffer, 0, bytesRead);
    var model = JsonSerializer.Deserialize<GameState>(jsonString);
    draw.ListDrawPoint(model.PlayerOnePosition);
    draw.ListDrawPoint(model.PlayerTwoPosition);
    draw.ListDrawPoint(model.WallsPosition);
}