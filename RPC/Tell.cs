using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aria2Sharp
{
    public partial class Aria2RPC
    {
        public Task<Aria2Info> TellStatus(string gid, params string[] keys)
            => Call<Aria2Info>("aria2.tellStatus", gid, TellKeysNormalize(keys));

        public Task<IEnumerable<Aria2Info>> TellActive(params string[] keys)
            => CallList<Aria2Info>("aria2.tellActive", TellKeysNormalize(keys));

        public Task<IEnumerable<Aria2Info>> TellWaiting(int offset = 0, int num = int.MaxValue, params string[] keys)
            => CallList<Aria2Info>("aria2.tellWaiting", offset, num, TellKeysNormalize(keys));

        public Task<IEnumerable<Aria2Info>> TellStopped(int offset = 0, int num = int.MaxValue, params string[] keys)
            => CallList<Aria2Info>("aria2.tellStopped", offset, num, TellKeysNormalize(keys));

        private JArray TellKeysNormalize(IEnumerable<string> keys)
            => JArray.FromObject(keys.Select(k => char.ToLowerInvariant(k[0]) + k.Substring(1)));

        public Task<IEnumerable<Aria2Uri>> GetUris(string gid) => CallList<Aria2Uri>("aria2.getUris", gid);
        public Task<IEnumerable<Aria2File>> GetFiles(string gid) => CallList<Aria2File>("aria2.getFiles", gid);
        public Task<IEnumerable<Aria2Peer>> GetPeers(string gid) => CallList<Aria2Peer>("aria2.getPeers", gid);
        public Task<IEnumerable<Aria2Server>> GetServers(string gid) => CallList<Aria2Server>("aria2.getServers", gid);
    }
}