using System.Collections.Generic;

namespace Aria2.NET
{
    public class Aria2Version
    {
        public string Version { get; set; }
        public IReadOnlyList<string> EnabledFeatures { get; set; }
    }
}