using System.Collections.Generic;

namespace Aria2Sharp
{
    public class Aria2Version
    {
        public string Version { get; set; }
        public IReadOnlyList<string> EnabledFeatures { get; set; }
    }
}