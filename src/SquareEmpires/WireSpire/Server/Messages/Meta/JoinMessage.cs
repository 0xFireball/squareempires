using Tempest;

namespace WireSpire.Server.Messages {
    public class JoinMessage : RemoteGameMessage {
        public JoinMessage() : base(MessageKind.Join) { }

        public bool ready = true;

        public override void WritePayload(ISerializationContext context, IValueWriter writer) {
            writer.WriteBool(ready);
        }

        public override void ReadPayload(ISerializationContext context, IValueReader reader) {
            ready = reader.ReadBool();
        }
    }
}