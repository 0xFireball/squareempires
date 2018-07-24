using System;
using System.Collections.Generic;
using Tempest;
using WireSpire.Server.Messages;

namespace WireSpire.Server {
    public static class RemoteGameProtocol {
        public const int TEMPEST_PROTOCOL_ID = 0x68;
        public static Protocol instance = new Protocol(TEMPEST_PROTOCOL_ID);

        static RemoteGameProtocol() {
            registerMessage<JoinMessage>();
            registerMessage<FinishTurnMessage>();
        }

        private static void registerMessage<TMessage>() where TMessage : Message, new() {
            instance.Register(new[] {new KeyValuePair<Type, Func<Message>>(typeof(TMessage), () => new TMessage())});
        }
    }
}