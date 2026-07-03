using System.Net;
using System.Net.Sockets;
using System.Text;
using SnakeGame.Domain.Options;


var gameOptions = GameOptions.Initialization();
var ipEndPont = IPEndPoint.Parse(gameOptions.Address);

var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
await client.ConnectAsync(ipEndPont);

while (true)
{
    byte[] readBuffer = new byte[1024];
    var data = await client.ReceiveAsync(readBuffer);
    Console.WriteLine(Encoding.UTF8.GetString(readBuffer, 0, data));
}