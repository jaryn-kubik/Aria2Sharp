using System.Collections.Generic;

namespace Aria2.NET
{
    public class Aria2File
    {
        public long Index { get; set; }
        public string Path { get; set; }

        public long Length { get; set; }
        public long CompletedLength { get; set; }
        public bool Selected { get; set; }

        public IReadOnlyList<Aria2Uri> Uris { get; set; }
    }
}