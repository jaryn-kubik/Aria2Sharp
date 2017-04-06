using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aria2.NET
{
    public partial class Aria2RPC
    {
        public Task<string> AddUri(string[] uris, IDictionary<Aria2Option, string> options = null, long position = long.MaxValue)
            => Call<string>("aria2.addUri", JArray.FromObject(uris), SerializeOptions(options), position);

        //TODO: addTorrent
        //TODO: addMetalink

        public async Task<IReadOnlyDictionary<Aria2Option, string>> GetOptions(string gid)
            => DeserializeOptions(await Call("aria2.getOption", gid));

        public Task<string> ChangeOptions(string gid, IDictionary<Aria2Option, string> options)
            => Call<string>("aria2.changeOption", gid, SerializeOptions(options));

        public async Task<IReadOnlyDictionary<Aria2Option, string>> GetGlobalOptions()
            => DeserializeOptions(await Call("aria2.getGlobalOption"));

        public Task<string> ChangeGlobalOptions(IDictionary<Aria2Option, string> options)
            => Call<string>("aria2.changeGlobalOption", SerializeOptions(options));

        private JObject SerializeOptions(IDictionary<Aria2Option, string> options)
        {
            JObject obj = new JObject();
            foreach (var option in options)
            {
                var key = option.Key.ToString().Replace('_', '-');
                obj[key] = option.Value;
            }
            return obj;
        }

        private IReadOnlyDictionary<Aria2Option, string> DeserializeOptions(JToken obj)
        {
            var options = new Dictionary<Aria2Option, string>();
            foreach (var option in (JObject)obj)
                if (Enum.TryParse(option.Key.Replace('-', '_'), out Aria2Option key))
                    options.Add(key, option.Value.ToString());
            return options;
        }
    }
}