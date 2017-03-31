using System;

namespace Aria2.NET
{
    public class Class1
    {
        public static void Main()
        {
            Aria2 aria = new Aria2();
            aria.ConnectAsync().Wait();
            var pica = aria.GetGlobalOptions().Result;
            //aria.AddUri(new[] { @"http://speedtest.ftp.otenet.gr/files/test10Mb.db" }).Wait();
            //aria.Call("aria2.addUri", new JArray(@"http://speedtest.ftp.otenet.gr/files/test10Mb.db"));
            //aria.TellStopped(0, int.MaxValue, nameof(Aria2Info.Status));
            Console.ReadLine();
            Console.ReadLine();
        }
    }
}