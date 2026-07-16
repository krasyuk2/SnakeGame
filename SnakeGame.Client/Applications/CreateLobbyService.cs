using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using SnakeGame.Domain.Applications;
using SnakeGame.Domain.Models;

namespace SnakeGame.Client.Applications;

/// <summary>
///     Сервис для создания лобби.
/// </summary>
public class CreateLobbyService
{
    /// <summary>
    ///     Сокет клиента.
    /// </summary>
    private readonly Socket _client;
    
    /// <summary>
    ///     Адрес сервера.
    /// </summary>
    private readonly IPEndPoint _ipEndPoint;
    
    /// <summary>
    ///     Конструктор.
    /// </summary>
    public CreateLobbyService(Socket client, IPEndPoint address)
    {
        _client = client;
        _ipEndPoint = address;
    }
    
    public async Task CreateLobby()
    {
        try
        {
            await _client.ConnectAsync(_ipEndPoint);
        }
        catch (Exception)
        {
            throw new Exception();
        }
        
        Console.Clear();
        Console.WriteLine("Введите название лобби:");
        var lobbyName = Console.ReadLine() ?? Guid.NewGuid().ToString();
        try
        {
            var createModel = new Lobby()
            {
                Name = lobbyName,
                Action = LobbyActions.Create,
                MaxPlayers = 2
            };
            var json = JsonSerializer.Serialize<Lobby>(createModel);
            await _client.SendMessageAsync(Encoding.UTF8.GetBytes(json));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}