using WireSpire.Types;

namespace WireSpire.Refs {
    public class BuildingRef {
        public Position pos;
        public int empire;
        public BuildingType type;
        public int level = 0;

        public BuildingRef() { }

        public BuildingRef(Building building) {
            pos = building.position;
            empire = building.empire.id;
            type = building.type;
            level = building.level;
        }
    }
}