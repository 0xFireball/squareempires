using System.Collections.Generic;
using Tempest;
using WireSpire.Refs;

namespace WireSpire.Server.Messages {
    public class EmpireFetchMessage : RemoteGameMessage {
        public EmpireFetchMessage() : base(MessageKind.EmpireFetch) { }

        public EmpireRef empire;
        public List<BuildingRef> buildings;

        public override void WritePayload(ISerializationContext context, IValueWriter writer) {
            writer.WriteString(empire.name);
            writer.WriteInt32(buildings.Count);
            foreach (var building in buildings) {
                writeBuilding(writer, building);
            }
        }

        public override void ReadPayload(ISerializationContext context, IValueReader reader) {
            empire = new EmpireRef();
            empire.name = reader.ReadString();
            buildings = new List<BuildingRef>();
            var buildingCount = reader.ReadInt32();
            for (var i = 0; i < buildingCount; i++) {
                var building = readBuilding(reader);
                buildings.Add(building);
            }
        }

        private void writeBuilding(IValueWriter writer, BuildingRef building) {
            writer.WriteInt32(building.empire);
            writer.WriteInt32((int) building.type);
            writer.WriteInt32(building.level);
            writer.writePosition(building.position);
        }

        private BuildingRef readBuilding(IValueReader reader) {
            return new BuildingRef {
                empire = reader.ReadInt32(),
                type = (BuildingRefType) reader.ReadInt32(),
                level = reader.ReadInt32(),
                position = reader.readPosition()
            };
        }
    }
}