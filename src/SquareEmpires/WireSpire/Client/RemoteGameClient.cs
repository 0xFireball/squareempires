using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tempest;
using WireSpire.Server.Messages;

namespace WireSpire.Client {
    public class RemoteGameClient : TempestClient {
        public RemoteGameClient(IClientConnection connection) : base(connection, MessageTypes.Reliable) { }

        private Dictionary<Type, Delegate> messageHandlers = new Dictionary<Type, Delegate>();

        public void subscribe<TMessage>(Action<TMessage> callback) where TMessage : RemoteGameMessage, new() {
            messageHandlers[typeof(TMessage)] = callback;
            this.RegisterMessageHandler<TMessage>(args => {
                messageHandlers[typeof(TMessage)].DynamicInvoke(args.Message);
            });
        }

        public Task sendMessageAsync(RemoteGameMessage msg) {
            return Connection.SendAsync(msg);
        }
    }
}