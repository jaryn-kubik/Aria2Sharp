using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aria2Sharp
{
    public class ClientWebSocketEx
    {
        private readonly ClientWebSocket webSocket = new ClientWebSocket();
        private readonly byte[] buffer = new byte[1024];
        private readonly TaskCompletionSource<bool> connected = new TaskCompletionSource<bool>();

        public event EventHandler<string> Message;
        public ClientWebSocketOptions Options => webSocket.Options;
        public bool Connected => connected.Task.IsCompleted;
        
        public Task SendAsync(string str)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);
            return webSocket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        public Task ConnectAsync(string url)
        {
            webSocket.ConnectAsync(new Uri(url), CancellationToken.None).ContinueWith(OnConnected);
            return connected.Task;
        }

        private async void OnConnected(Task task)
        {
            connected.SetResult(true);
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