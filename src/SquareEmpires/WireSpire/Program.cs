using System.Collections.Generic;
using Tempest;
using Tempest.Providers.Network;
using WireSpire.Server;

namespace WireSpire {
    public static class Program {
        public static void Main(string[] args) {
            var opts = options(args);
            opts.TryGetValue("p", out var portStr);
            int.TryParse(portStr ?? "14834", out var serverPort);
            var server = new GameServer(new NetworkConnectionProvider(RemoteGameProtocol.instance,
                new Target(Target.AnyIP, serverPort), GameServer.MAX_CONNECTIONS));
            server.initializeSimulation();
            server.Start();
        }

        public static Dictionary<string, string> options(string[] args) {
            var opts = new Dictionary<string, string>();
            foreach (var arg in args) {
                var parts = arg.Split('=');
                var id = parts[0].Substring(1);
                var val = parts[1];
                opts[id] = val;
            }

            return opts;
        }
    }
}