using System.Net;
using System.Net.Sockets;

namespace SnakeGame.Domain.Applications;

public static class SocketExtensions
{
    extension(Socket socket)
    {
        /// <summary>
        ///     Принять сообщение, сперва получаем int, как длину сообщение
        ///     После получаем данные по полученной длине.
        /// </summary>
        /// <returns> Массив Byte. </returns>
        public async Task<byte[]> ReceiveMessageAsync()
        {
            byte[] bufferLenght = new byte[4];
            await ReceiveExtractAsync(socket, bufferLenght);
            int length = BitConverter.ToInt32(bufferLenght, 0);
            byte[] bufferMessage = new byte[length];
            await ReceiveExtractAsync(socket, bufferMessage);
            return bufferMessage;
        }

        /// <summary>
        ///     Отправляем сперва длину сообщения, а после само сообщение.
        /// </summary>
        /// <param name="message"> Массив Byte - Данные. </param>
        public async Task SendMessageAsync(byte[] message)
        {
            byte[] length = BitConverter.GetBytes(message.Length); // 4 byte
            await socket.SendAsync(length);
            await socket.SendAsync(message);
        }
    }

    /// <summary>
    ///     Принимаем по указанной длине буфера сообщение, Пока не заполнится вся длинна. 
    /// </summary>
    /// <param name="socket"> Сокет. </param>
    /// <param name="buffer"> Буфер для данных. </param>
    public static async Task ReceiveExtractAsync(Socket socket, byte[] buffer)
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
}