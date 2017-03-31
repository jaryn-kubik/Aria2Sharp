using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aria2.NET
{
    public partial class Aria2
    {
        private readonly ClientWebSocketEx webSocket = new ClientWebSocketEx();
        private readonly Dictionary<long, TaskCompletionSource<JToken>> waiters = new Dictionary<long, TaskCompletionSource<JToken>>();

        public EventHandler<Aria2NotificationEventArgs> Notification;
        public EventHandler<Aria2Exception> Error;

        public Aria2()//TODO: secret
        {
            webSocket.Options.KeepAliveInterval = TimeSpan.MaxValue;
            webSocket.Message += OnMessage;
        }

        public Task ConnectAsync(string url = "ws://localhost:6800/jsonrpc") => webSocket.ConnectAsync(url);

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
                else if (waiters.TryGetValue(id.Value, out var waiter))
                {
                    if (result.TryGetValue("error", out JToken error))
                        waiter.SetException(new Aria2Exception(error["code"], error["message"]));
                    else
                        waiter.SetResult(result["result"]);
                    waiters.Remove(id.Value);
                }
            }
        }

        private long currentId;
        private async Task<JToken> Call(string method, params JToken[] args)
        {
            long id = currentId++;
            JObject obj = new JObject
            {
                ["jsonrpc"] = "2.0",
                ["id"] = id.ToString(),
                ["method"] = method,
                ["params"] = JArray.FromObject(args)
            };

            TaskCompletionSource<JToken> waiter = new TaskCompletionSource<JToken>();
            waiters.Add(id, waiter);
            await webSocket.SendAsync(obj.ToString(Formatting.None));
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
