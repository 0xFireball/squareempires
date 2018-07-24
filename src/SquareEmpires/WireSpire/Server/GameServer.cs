using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace WireSpire.Server {
    public class GameServer {
        public IPEndPoint endpoint;
        public ConcurrentBag<ConnectedPlayer> players;

        public GameServer(IPEndPoint endpoint, int playerCount) {
            this.endpoint = endpoint;
        }

        public async Task start(CancellationToken cancellationToken) {
            var listenTask = listen(cancellationToken);
        }

        public async Task listen(CancellationToken cancellationToken) {
            var listener = new TcpListener(endpoint);
            listener.Start();
            while (!cancellationToken.IsCancellationRequested) {
                var client = await listener.AcceptTcpClientAsync();
                // TODO: get the player id some better way
                // TODO: for MP matches some sort of join code
                var player = new ConnectedPlayer(players.Count, client);
                players.Add(player);
                player.sock.Client.BeginReceive(player.buffer, 0, ConnectedPlayer.BUFFER_SIZE, 0,
                    readCallback, player);
            }
        }

        private void readCallback(IAsyncResult ar) {
            var player = (ConnectedPlayer) ar.AsyncState;
            var readCount = player.sock.Client.EndReceive(ar);
            // process the data?
            // TODO: Check turn for inputs
            // TODO: lock the game state (threading crap)
            // start another receive
            player.sock.Client.BeginReceive(player.buffer, 0, ConnectedPlayer.BUFFER_SIZE, 0,
                readCallback, player);
        }
    }
}