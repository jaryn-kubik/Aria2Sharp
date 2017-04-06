using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Aria2Sharp
{
    public partial class Aria2RPC
    {
        public Task Remove(string gid) => Call("aria2.remove", gid);
        public Task ForceRemove(string gid) => Call("aria2.forceRemove", gid);

        public Task Pause(string gid) => Call("aria2.pause", gid);
        public Task PauseAll() => Call("aria2.pauseAll");

        public Task ForcePause(string gid) => Call("aria2.forcePause", gid);
        public Task ForcePauseAll() => Call("aria2.forcePauseAll");

        public Task Unpause(string gid) => Call("aria2.unpause", gid);
        public Task UnpauseAll() => Call("aria2.unpauseAll");

        public Task PurgeDownloadResult() => Call("aria2.purgeDownloadResult");
        public Task RemoveDownloadResult(string gid) => Call("aria2.removeDownloadResult", gid);

        public enum PositionType { POS_SET, POS_CUR, POS_END }
        public Task<long> ChangePosition(string gid, long pos, PositionType how) => Call<long>("aria2.changePosition", gid, pos, how.ToString());

        public async Task<Tuple<long, long>> ChangeUri(string gid, long fileIndex, string[] delUris, string[] addUris)
        {
            var result = await CallList<long>("aria2.changeUri", gid, fileIndex, JArray.FromObject(delUris), JArray.FromObject(addUris));
            var array = result.ToArray();
            return new Tuple<long, long>(array[0], array[1]);
        }
    }
}