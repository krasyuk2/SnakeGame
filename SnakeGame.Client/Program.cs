using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using SnakeGame.Client.Models;
using SnakeGame.Domain.Applications;
using SnakeGame.Domain.Models;
using SnakeGame.Domain.Options;
using SnakeGame.Models;

var gameOptions = GameOptions.Initialization();

const int panelWidth = 30;
var panelX = gameOptions.MapSize.Width + 2;
var panelRightBorderX = gameOptions.MapSize.Width + panelWidth - 2;
var panelTextWidth = panelWidth - 4;

if (OperatingSystem.IsWindows())
{
    var windowWidth = gameOptions.MapSize.Width + panelWidth;
    var windowHeight = gameOptions.MapSize.Height + 1;
    
    Console.SetWindowSize(1, 1); 
    Console.SetBufferSize(windowWidth, windowHeight);
    Console.SetWindowSize(windowWidth, windowHeight);    
}

var ipEndPont = IPEndPoint.Parse(gameOptions.Address);

var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
await client.ConnectAsync(ipEndPont);
Draw draw = new Draw();

var panelBottomY = gameOptions.MapSize.Height - 1;
var borderTop = Enumerable.Range(gameOptions.MapSize.Width, panelWidth - 1)
    .Select(x => new Point(x, 0, x == gameOptions.MapSize.Width || x == panelRightBorderX ? '+' : '-', 0));
var borderBottom = Enumerable.Range(gameOptions.MapSize.Width, panelWidth - 1)
    .Select(x => new Point(x, panelBottomY, x == gameOptions.MapSize.Width || x == panelRightBorderX ? '+' : '-', 0));
var borderLeft = Enumerable.Range(1, panelBottomY - 1)
    .Select(y => new Point(gameOptions.MapSize.Width, y, '|', 0));
var borderRight = Enumerable.Range(1, panelBottomY - 1)
    .Select(y => new Point(panelRightBorderX, y, '|', 0));

var panelBorder = borderTop.Concat(borderBottom).Concat(borderLeft).Concat(borderRight);
foreach (var point in panelBorder)
{
    draw.SingleDrawPoint(point);
}

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

    draw.DrawText(panelX, 2, $"Игрок 1 : {model.PlayerOnePosition.Count()}");
    draw.DrawText(panelX, 3, $"Игрок 2 : {model.PlayerTwoPosition.Count()}");
    draw.DrawText(panelX, 4, $"Очков для победы : {gameOptions.GameNumberWin}");
}