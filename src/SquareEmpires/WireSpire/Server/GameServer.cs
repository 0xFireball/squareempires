using System.Collections.Generic;
using System.Linq;
using Tempest;
using WireSpire.Server.Messages;
using WireSpire.Types;

namespace WireSpire.Server {
    public class GameServer : TempestServer {
        public const int MAX_CONNECTIONS = 256;
        private readonly List<ConnectedPlayer> players = new List<ConnectedPlayer>();

        public Simulation simulation;

        public GameServer(IConnectionProvider provider) : base(provider, MessageTypes.Reliable) {
            this.RegisterMessageHandler<JoinMessage>(onJoinMessage);
        }

        public void initializeSimulation() {
            // TODO: WTF are these numbers lol
            simulation = new Simulation(1, new Position(8, 8));
            simulation.initialize(); // hmmm
        }

        private void onJoinMessage(MessageEventArgs<JoinMessage> obj) {
            // assign the empire or something
            // TODO: this should properly support picking _your_ empire on a save
            var player = players.First(x => x.connection == obj.Connection);
            player.empireId = player.id;
            obj.Connection.SendAsync(new GameInfoMessage {
                playerCount = simulation.empires.Count,
                mapSize = simulation.world.map.size,
                empireId = player.empireId
            });
        }

        protected override void OnConnectionMade(object sender, ConnectionMadeEventArgs e) {
            lock (players) { players.Add(new ConnectedPlayer(players.Count, e.Connection)); }

            base.OnConnectionMade(sender, e);
        }

        protected override void OnConnectionDisconnected(object sender, DisconnectedEventArgs e) {
            lock (players) { players.RemoveAll(x => x.connection == e.Connection); }

            base.OnConnectionDisconnected(sender, e);
        }
    }

//    public class GameServer {
//        public IPEndPoint endpoint;
//        public ConcurrentBag<ConnectedPlayer> players;
//
//        public GameServer(IPEndPoint endpoint, int playerCount) {
//            this.endpoint = endpoint;
//        }
//
//        public async Task start(CancellationToken cancellationToken) {
//            var listenTask = listen(cancellationToken);
//        }
//
//        public async Task listen(CancellationToken cancellationToken) {
//            var listener = new TcpListener(endpoint);
//            listener.Start();
//            while (!cancellationToken.IsCancellationRequested) {
//                var client = await listener.AcceptTcpClientAsync();
//                // TODO: get the player id some better way
//                // TODO: for MP matches some sort of join code
//                var player = new ConnectedPlayer(players.Count, client);
//                players.Add(player);
//                player.sock.Client.BeginReceive(player.buffer, 0, ConnectedPlayer.BUFFER_SIZE, 0,
//                    readCallback, player);
//            }
//        }
//
//        private void readCallback(IAsyncResult ar) {
//            var player = (ConnectedPlayer) ar.AsyncState;
//            var readCount = player.sock.Client.EndReceive(ar);
//            // process the data?
//            // TODO: Check turn for inputs
//            // TODO: lock the game state (threading crap)
//            // start another receive
//            player.sock.Client.BeginReceive(player.buffer, 0, ConnectedPlayer.BUFFER_SIZE, 0,
//                readCallback, player);
//        }
//    }
}