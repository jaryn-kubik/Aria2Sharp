using System.Collections.Generic;

namespace Aria2Sharp
{
    public class Aria2Server
    {
        public long Index { get; set; }
        public IReadOnlyList<Aria2ServerInfo> Servers { get; set; }
    }

    public class Aria2ServerInfo
    {
        public string Uri { get; set; }
        public string CurrentUri { get; set; }
        public long DownloadSpeed { get; set; }
    }
}