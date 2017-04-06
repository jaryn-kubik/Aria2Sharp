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
        private readonly string url;
        private TaskCompletionSource<bool> connected;

        public event EventHandler<string> Message;
        public ClientWebSocketOptions Options => webSocket.Options;

        public ClientWebSocketEx(string url) { this.url = url; }

        public async Task SendAsync(string str)
        {
            if (connected == null)
            {
                connected = new TaskCompletionSource<bool>();
                await webSocket.ConnectAsync(new Uri(url), CancellationToken.None).ContinueWith(OnConnected);
            }
            await connected.Task;

            byte[] data = Encoding.UTF8.GetBytes(str);
            await webSocket.SendAsync(new ArraySegment<byte>(data), WebSocketMessageType.Text, true, CancellationToken.None);
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