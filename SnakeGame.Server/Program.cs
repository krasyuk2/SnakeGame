using System.Net;
using System.Net.Sockets;
using System.Text;
using SnakeGame.Domain.Options;
using SnakeGame.Models;
 
var gameOptions = GameOptions.Initialization();

Walls walls = new Walls(20,20);
walls.GenerateWalls();
var wallsPos = walls.GetWallsPoint();

var ipEndPont = IPEndPoint.Parse(gameOptions.Address);
var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
server.Bind(ipEndPont);
server.Listen(2);

var player = await server.AcceptAsync();
while (true)
{
    await Task.Delay(1000);
    byte[] message = Encoding.UTF8.GetBytes("Hello World!");
    await player.SendAsync(message);
}