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
    byte[] data = await ReceiveMessageAsync(client);
    string jsonString = Encoding.UTF8.GetString(data);
    var model = JsonSerializer.Deserialize<GameState>(jsonString);
    Console.Clear();
    draw.ListDrawPoint(model.PlayerOnePosition);
    draw.ListDrawPoint(model.PlayerTwoPosition);
    draw.ListDrawPoint(model.WallsPosition);
    draw.SingleDrawPoint(model.FoodPosition);
}


async Task<byte[]> ReceiveMessageAsync(Socket socket)
{
    byte[] bufferLenght = new byte[4];
    await ReceiveExtractAsync(socket, bufferLenght);
    int length = BitConverter.ToInt32(bufferLenght, 0);
    byte[] bufferMessage = new byte[length];
    await ReceiveExtractAsync(socket, bufferMessage);
    return bufferMessage;
}

async Task ReceiveExtractAsync(Socket socket, byte[] buffer)
{
    int totalRead = 0;
    while (totalRead < buffer.Length)
    {
        int bytesRead = await socket.ReceiveAsync(buffer.AsMemory(totalRead));
        if(bytesRead == 0)
            throw new SocketException(500, "Соединение разорвано");
        totalRead += bytesRead;
    }
}