using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aria2.NET
{
    public partial class Aria2
    {
        public Task<Aria2Stats> GetGlobalStat() => Call<Aria2Stats>("aria2.getGlobalStat");
        public Task<Aria2Version> GetVersion() => Call<Aria2Version>("aria2.getVersion");
        public async Task<string> GetSessionInfo() => (await Call("aria2.getSessionInfo"))["sessionId"].ToString();

        public Task Shutdown() => Call("aria2.shutdown");
        public Task ForceShutdown() => Call("aria2.forceShutdown");
        public Task SaveSession() => Call("aria2.saveSession");

        public Task<IEnumerable<string>> ListMethods() => CallList<string>("system.listMethods");
        public Task<IEnumerable<string>> ListNotifications() => CallList<string>("system.listNotifications");
    }
}