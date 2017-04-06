using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Aria2Sharp
{
    public partial class Aria2RPC
    {
        private readonly ClientWebSocketEx webSocket;
        private event Action<long, JToken, JToken> Message;

        public event EventHandler<Aria2NotificationEventArgs> Notification;
        public event EventHandler<Aria2Exception> Error;
        public string Secret { get; set; }
        public string Host { get; }
        public long Port { get; }
        public bool Secure { get; }

        public Aria2RPC(string host = "localhost", ushort port = 6800, bool secure = false)
        {
            Host = host;
            Port = port;
            Secure = secure;
            var protocol = secure ? "wss" : "ws";
            webSocket = new ClientWebSocketEx($"{protocol}://{host}:{port}/jsonrpc");
            webSocket.Options.KeepAliveInterval = TimeSpan.MaxValue;
            webSocket.Message += OnMessage;
        }

        private void OnMessage(object sender, string msg)
        {
            var result = JObject.Parse(msg);
            if (result["id"] == null)
            {
                var method = result["method"].ToString();
                var gid = result["params"][0]["gid"].ToString();
                Notification?.Invoke(this, new Aria2NotificationEventArgs(method, gid));
            }
            else
            {
                var id = result["id"].ToObject<long?>();
                if (!id.HasValue)
                {
                    var error = result["error"];
                    Error?.Invoke(this, new Aria2Exception(error["code"], error["message"]));
                }
                else
                    Message?.Invoke(id.Value, result["result"], result["error"]);
            }
        }

        private long currentId;
        private async Task<JToken> Call(string method, params JToken[] args)
        {
            long id = currentId++;
            JArray parameters = new JArray();
            if (!string.IsNullOrEmpty(Secret))
                parameters.Add($"token:{Secret}");
            foreach (var arg in args)
                parameters.Add(arg);
            JObject obj = new JObject
            {
                ["jsonrpc"] = "2.0",
                ["id"] = id.ToString(),
                ["method"] = method,
                ["params"] = parameters
            };

            TaskCompletionSource<JToken> waiter = new TaskCompletionSource<JToken>();
            void msgHandler(long msgId, JToken result, JToken error)
            {
                if (msgId != id)
                    return;
                try
                {
                    if (error != null)
                        waiter.TrySetException(new Aria2Exception(error["code"], error["message"]));
                    else
                        waiter.TrySetResult(result);
                }
                finally { Message -= msgHandler; }
            }
            Message += msgHandler;
            await webSocket.SendAsync(obj.ToString(Formatting.None));

            var cts = new CancellationTokenSource();
            cts.CancelAfter(15000);
            cts.Token.Register(o =>
            {
                Message -= msgHandler;
                ((TaskCompletionSource<JToken>)o).TrySetCanceled();
            }, waiter);
            return await waiter.Task;
        }

        private async Task<T> Call<T>(string method, params JToken[] args)
        {
            var result = await Call(method, args);
            return result.ToObject<T>();
        }

        private async Task<IEnumerable<T>> CallList<T>(string method, params JToken[] args)
        {
            var result = await Call(method, args);
            return result.Select(r => r.ToObject<T>());
        }
    }
}