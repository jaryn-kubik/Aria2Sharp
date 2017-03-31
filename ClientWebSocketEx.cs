using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aria2.NET
{
    public class ClientWebSocketEx
    {
        private readonly ClientWebSocket webSocket = new ClientWebSocket();
        private readonly byte[] buffer = new byte[1024];

        public EventHandler<string> Message;
        public ClientWebSocketOptions Options => webSocket.Options;

        public Task SendAsync(string str)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);
            return webSocket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public Task ConnectAsync(string url)
        {
            var connected = new TaskCompletionSource<bool>();
            webSocket.ConnectAsync(new Uri(url), CancellationToken.None).ContinueWith(OnConnected, connected);
            return connected.Task;
        }

        private async void OnConnected(Task task, object state)
        {
            ((TaskCompletionSource<bool>)state).SetResult(true);
            var str = new StringBuilder();
            while (webSocket.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result;
                do
                {
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    str.Append(Encoding.UTF8.GetString(buffer, 0, result.Count));
                } while (!result.EndOfMessage);

                Message?.Invoke(this, str.ToString());
                str.Clear();
            }
        }
    }
}