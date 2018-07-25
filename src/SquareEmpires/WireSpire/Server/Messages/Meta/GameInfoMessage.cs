using System.Collections.Generic;
using Tempest;
using WireSpire.Refs;
using WireSpire.Types;

namespace WireSpire.Server.Messages {
    public class GameInfoMessage : RemoteGameMessage {
        public GameInfoMessage() : base(MessageKind.GameInfo) { }

        public int empireCount;
        public Position mapSize;
        public int empireId;
        public List<EmpireRef> empires;

        public override void WritePayload(ISerializationContext context, IValueWriter writer) {
            writer.WriteInt32(empireCount);
            writer.writePosition(mapSize);
            writer.WriteInt32(empireId);
            foreach (var empire in empires) {
                writer.WriteInt32(empire.id);
                writer.WriteString(empire.name);
            }
        }

        public override void ReadPayload(ISerializationContext context, IValueReader reader) {
            empireCount = reader.ReadInt32();
            mapSize = reader.readPosition();
            empireId = reader.ReadInt32();
            empires = new List<EmpireRef>();
            for (var i = 0; i < empireCount; i++) {
                empires.Add(new EmpireRef {
                    id = reader.ReadInt32(),
                    name = reader.ReadString()
                });
            }
        }
    }
}