using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace Aria2Sharp
{
    public partial class Aria2RPC
    {
        public Task<string> AddUri(string[] uris, Aria2Options options = null, long position = long.MaxValue)
            => Call<string>("aria2.addUri", JArray.FromObject(uris), SerializeOptions(options), position);

        //TODO: addTorrent
        //TODO: addMetalink

        public async Task<string> GetOption(string gid, Aria2Option option) => (await GetOptions(gid))[option];
        public async Task<Aria2Options> GetOptions(string gid) => DeserializeOptions(await Call("aria2.getOption", gid));

        public async Task<string> GetGlobalOption(Aria2Option option) => (await GetGlobalOptions())[option];
        public async Task<Aria2Options> GetGlobalOptions() => DeserializeOptions(await Call("aria2.getGlobalOption"));

        public Task<string> ChangeOptions(string gid, Aria2Options options)
            => Call<string>("aria2.changeOption", gid, SerializeOptions(options));

        public Task<string> ChangeOption(string gid, Aria2Option option, string value)
            => ChangeOptions(gid, new Aria2Options { [option] = value });

        public Task<string> ChangeGlobalOptions(Aria2Options options)
            => Call<string>("aria2.changeGlobalOption", SerializeOptions(options));

        public Task<string> ChangeGlobalOption(Aria2Option option, string value)
            => ChangeGlobalOptions(new Aria2Options { [option] = value });

        private JObject SerializeOptions(Aria2Options options)
        {
            JObject obj = new JObject();
            foreach (var option in options)
            {
                var key = option.Key.ToString().Replace('_', '-');
                obj[key] = option.Value;
            }
            return obj;
        }

        private Aria2Options DeserializeOptions(JToken obj)
        {
            var options = new Aria2Options();
            foreach (var option in (JObject)obj)
                if (Enum.TryParse(option.Key.Replace('-', '_'), out Aria2Option key))
                    options.Add(key, option.Value.ToString());
            return options;
        }
    }
}