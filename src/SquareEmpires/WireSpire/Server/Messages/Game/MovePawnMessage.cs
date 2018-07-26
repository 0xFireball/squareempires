using Tempest;
using WireSpire.Refs;
using WireSpire.Types;

namespace WireSpire.Server.Messages {
    public class MovePawnMessage : RemoteGameMessage {
        public MovePawnMessage() : base(MessageKind.MovePawn) { }

        public PawnRef pawn;
        public Position dest;
        
        public override void WritePayload(ISerializationContext context, IValueWriter writer) {
            writer.writePawn(pawn);
            writer.writePosition(dest);
        }

        public override void ReadPayload(ISerializationContext context, IValueReader reader) {
            pawn = reader.readPawn();
            dest = reader.readPosition();
        }
    }
}