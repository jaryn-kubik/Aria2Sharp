using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aria2Sharp
{
    public class ClientWebSocketEx
    {
        private readonly ClientWebSocket webSocket = new ClientWebSocket();
        private readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        private readonly string url;

        public event EventHandler<string> Message;
        public event EventHandler<Exception> Error;
        public ClientWebSocketOptions Options => webSocket.Options;

        public ClientWebSocketEx(string url) { this.url = url; }

        public async Task SendAsync(string str)
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                if (webSocket.State != WebSocketState.Open)
                {
                    await webSocket.ConnectAsync(new Uri(url), CancellationToken.None);
                    StartListening();
                }
                byte[] data = Encoding.UTF8.GetBytes(str);
                await webSocket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            finally { semaphoreSlim.Release(); }
        }

        private void StartListening() => Task.Run(ReceiveLoop);
        private async Task ReceiveLoop()
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                byte[] buffer = new byte[1024];
                while (webSocket.State == WebSocketState.Open)
                {
                    WebSocketReceiveResult result;
                    do
                    {
                        result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        ms.Write(buffer, 0, result.Count);
                    } while (!result.EndOfMessage);

                    Message?.Invoke(this, Encoding.UTF8.GetString(ms.GetBuffer(), 0, (int)ms.Length));
                    ms.SetLength(0);
                }
            }
            catch (Exception e) { Error?.Invoke(this, e); }
        }
    }
}