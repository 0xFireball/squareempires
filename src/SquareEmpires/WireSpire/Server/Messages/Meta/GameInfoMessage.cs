using Tempest;
using WireSpire.Types;

namespace WireSpire.Server.Messages {
    public class GameInfoMessage : RemoteGameMessage {
        public GameInfoMessage() : base(MessageKind.GameInfo) {}

        public int playerCount;
        public Position mapSize;
        public int empireId;
        
        public override void WritePayload(ISerializationContext context, IValueWriter writer) {
            writer.WriteInt32(playerCount);
            writer.WriteInt32(mapSize.x);
            writer.WriteInt32(mapSize.y);
            writer.WriteInt32(empireId);
        }

        public override void ReadPayload(ISerializationContext context, IValueReader reader) {
            playerCount = reader.ReadInt32();
            mapSize = new Position(reader.ReadInt32(), reader.ReadInt32());
            empireId = reader.ReadInt32();
        }
    }
}