using System.Collections.Generic;

namespace Aria2Sharp
{
    public class Aria2Bittorent
    {
        public IReadOnlyList<string> AnnounceList { get; set; }
        public string Comment { get; set; }
        public long CreationDate { get; set; }
        public FileMode Mode { get; set; }

        public enum FileMode { Single, Multi }
    }
}