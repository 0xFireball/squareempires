using System.Collections.Generic;
using Tempest;
using WireSpire.Types;

namespace WireSpire.Server.Messages {
    public class GameInfoMessage : RemoteGameMessage {
        public GameInfoMessage() : base(MessageKind.GameInfo) { }

        public int empireCount;
        public Position mapSize;
        public int empireId;
        public List<string> empireNames;

        public override void WritePayload(ISerializationContext context, IValueWriter writer) {
            writer.WriteInt32(empireCount);
            writer.writePosition(mapSize);
            writer.WriteInt32(empireId);
            foreach (var empireName in empireNames) {
                writer.WriteString(empireName);
            }
        }

        public override void ReadPayload(ISerializationContext context, IValueReader reader) {
            empireCount = reader.ReadInt32();
            mapSize = reader.readPosition();
            empireId = reader.ReadInt32();
            empireNames = new List<string>();
            for (var i = 0; i < empireCount; i++) {
                empireNames.Add(reader.ReadString());
            }
        }
    }
}