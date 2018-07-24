using Tempest;

namespace WireSpire.Server.Messages {
    public class FinishTurnMessage : RemoteGameMessage {
        public FinishTurnMessage() : base(MessageKind.FinishTurn) { }
        
        public override void WritePayload(ISerializationContext context, IValueWriter writer) { }

        public override void ReadPayload(ISerializationContext context, IValueReader reader) { }
    }
}