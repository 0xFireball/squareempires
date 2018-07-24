using Tempest;

namespace WireSpire.Server.Messages {
    public abstract class RemoteGameMessage : Message {
        protected RemoteGameMessage(MessageKind kind) : base(RemoteGameProtocol.instance, (ushort) kind) { }
    }
}