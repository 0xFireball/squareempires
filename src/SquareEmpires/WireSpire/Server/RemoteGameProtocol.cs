using System;
using System.Collections.Generic;
using System.Reflection;
using Tempest;

namespace WireSpire.Server {
    public static class RemoteGameProtocol {
        public const int TEMPEST_PROTOCOL_ID = 0x68;
        public static Protocol instance = new Protocol(TEMPEST_PROTOCOL_ID);

        static RemoteGameProtocol() {
            discoverTypes();
        }

        private static void discoverTypes() {
            var assembly = Assembly.GetExecutingAssembly();
            foreach (var type in assembly.GetTypes()) {
                if (!type.IsAbstract && typeof(Message).IsAssignableFrom(type)) {
                    instance.Register(new[]
                        {new KeyValuePair<Type, Func<Message>>(type, () => (Message) Activator.CreateInstance(type))});
                }
            }
        }

        private static void registerMessage<TMessage>() where TMessage : Message, new() {
            instance.Register(new[] {new KeyValuePair<Type, Func<Message>>(typeof(TMessage), () => new TMessage())});
        }
    }
}