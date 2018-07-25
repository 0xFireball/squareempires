using System.Collections.Generic;
using Tempest;
using WireSpire.Refs;

namespace WireSpire.Server.Messages {
    public class EmpireFetchMessage : RemoteGameMessage {
        public EmpireFetchMessage() : base(MessageKind.EmpireFetch) { }

        public EmpireRef empireRef;
        public List<BuildingRef> buildings;

        public EmpireFetchMessage(Empire empire) : this() {
            empireRef = new EmpireRef(empire);
            buildings = new List<BuildingRef>();
            foreach (var building in empire.buildings) {
                buildings.Add(new BuildingRef(building));
            }
        }

        public override void WritePayload(ISerializationContext context, IValueWriter writer) {
            writer.WriteString(empireRef.name);
            writer.WriteInt32(buildings.Count);
            foreach (var building in buildings) {
                writer.writeBuilding(building);
            }
        }

        public override void ReadPayload(ISerializationContext context, IValueReader reader) {
            empireRef = new EmpireRef {name = reader.ReadString()};
            buildings = new List<BuildingRef>();
            var buildingCount = reader.ReadInt32();
            for (var i = 0; i < buildingCount; i++) {
                var building = reader.readBuilding();
                buildings.Add(building);
            }
        }
    }
}