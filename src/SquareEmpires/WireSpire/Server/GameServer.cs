﻿using System.Collections.Generic;
using System.Linq;
using Tempest;
using WireSpire.Entities;
using WireSpire.Refs;
using WireSpire.Server.Mech;
using WireSpire.Server.Messages;
using WireSpire.Types;

namespace WireSpire.Server {
    public class GameServer : TempestServer {
        public const int MAX_CONNECTIONS = 256;
        private readonly List<ConnectedPlayer> players = new List<ConnectedPlayer>();

        public Simulation simulation;

        public GameServer(IConnectionProvider provider) : base(provider, MessageTypes.Reliable) {
            this.RegisterMessageHandler<JoinMessage>(onJoinMessage);
            this.RegisterMessageHandler<MovePawnMessage>(onMovePawnMessage);
            this.RegisterMessageHandler<FinishTurnMessage>(onFinishTurnMessage);
        }

        public void initializeSimulation() {
            // TODO: WTF are these numbers lol
            simulation = new Simulation(1, new Position(8, 8));
            simulation.initialize(); // hmmm
        }

        private void onJoinMessage(MessageEventArgs<JoinMessage> msg) {
            // assign the empire or something
            // TODO: this should properly support picking _your_ empire on a save
            var player = players.First(x => x.connection == msg.Connection);
            player.empireId = player.id;
            var empire = simulation.empires[player.empireId];
            msg.Connection.SendAsync(new GameInfoMessage {
                empireCount = simulation.empires.Count,
                mapSize = simulation.world.map.size,
                empireId = player.empireId,
                empires = simulation.empires.Select(x => new EmpireRef(x)).ToList()
            });
            msg.Connection.SendAsync(new EmpireFetchMessage(empire));
            var observedWorld = new ObservedWorld(simulation.world, empire, simulation.time);
            observedWorld.see();
            msg.Connection.SendAsync(new WorldUpdateMessage {world = observedWorld});
        }

        private void onMovePawnMessage(MessageEventArgs<MovePawnMessage> msg) {
            // find our matching pawn repr
            var pawn = simulation.empires.SelectMany(x => x.pawns)
                .FirstOrDefault(x => x.pos.equalTo(msg.Message.pawn.pos));
            if (pawn == null) return;
            // sanity check (bounds)
            if (!simulation.world.inWorld(msg.Message.dest)) return;
            // enforce distance and time check
            if (pawn.lastMove < simulation.time) {
                if (Position.chDist(pawn.pos, msg.Message.dest) < Pawn.vision[pawn.type]) {
                    // move approved
                    pawn.lastMove = simulation.time;
                    pawn.pos = msg.Message.dest;
                    sendWorldUpdates();
                }
            }
        }

        private void onFinishTurnMessage(MessageEventArgs<FinishTurnMessage> msg) {
            // TODO: step the simulation? ensure
            simulation.step();
            sendWorldUpdates(); // send updated world to everyone
            // TODO: anything else?
        }

        private void sendWorldUpdates() {
            // send an update to everyone
            lock (players) {
                foreach (var player in players) {
                    var observedWorld = new ObservedWorld(simulation.world, simulation.empires[player.empireId],
                        simulation.time);
                    observedWorld.see();
                    player.connection.SendAsync(new WorldUpdateMessage {world = observedWorld});
                }
            }
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
//        public GameServer(IPEndPoint endpoint, int empireCount) {
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