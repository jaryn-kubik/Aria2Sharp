using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Aria2Sharp
{
    public class Aria2
    {
        public Aria2RPC RPC { get; }
        public Process Process { get; private set; }

        public event EventHandler<string> Output;
        public event EventHandler<string> Error;

        public Aria2(string host = "localhost", ushort port = 6800, bool secure = false)
        {
            var processes = Process.GetProcessesByName("aria2c");
            if (processes.Length > 0)
                Process = processes[0];
            RPC = new Aria2RPC(host, port, secure);
        }

        public void Start(Aria2Options args)
        {
            if (args.TryGetValue(Aria2Option.input_file, out string input_file))
                if (!File.Exists(input_file))
                    File.Create(input_file);

            if (args.TryGetValue(Aria2Option.dir, out string dir))
                args[Aria2Option.dir] = $"\"{dir.TrimEnd('\\')}\"";

            args[Aria2Option.enable_rpc] = "true";
            args[Aria2Option.enable_color] = "false";
            args[Aria2Option.rpc_listen_port] = RPC.Port.ToString();
            args[Aria2Option.rpc_secure] = RPC.Secure ? "true" : "false";

            string arguments = args.Aggregate("", (str, a) => $"{str} --{a.Key.ToString().Replace('_', '-')}={a.Value}");
            Process = new Process
            {
                StartInfo = new ProcessStartInfo("aria2c.exe")
                {
                    Arguments = arguments,
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                }
            };
            Process.OutputDataReceived += OnOutput;
            Process.ErrorDataReceived += OnError;
            Process.Start();
            Process.BeginOutputReadLine();
            Process.BeginErrorReadLine();
        }

        private void OnOutput(object sender, DataReceivedEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.Data))
                Output?.Invoke(this, args.Data);
        }

        private void OnError(object sender, DataReceivedEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.Data))
                Error?.Invoke(this, args.Data);
        }
    }
}
