using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aria2.NET
{
    public class Aria2
    {
        public Aria2RPC RPC { get; }

        public Aria2()
        {
            RPC = new Aria2RPC();
        }
    }
}
