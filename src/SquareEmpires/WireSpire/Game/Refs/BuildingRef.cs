using WireSpire.Types;

namespace WireSpire.Refs {
    public class BuildingRef : ThingRef {
        public Building.Type type;
        public int level = 0;

        public BuildingRef() { }

        public BuildingRef(Building building) {
            pos = building.pos;
            empire = building.empire.id;
            type = building.type;
            level = building.level;
        }
    }
}