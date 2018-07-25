using WireSpire.Types;

namespace WireSpire.Refs {
    public class BuildingRef {
        public Position position;
        public int empire;
        public BuildingType type;
        public int level = 0;

        public BuildingRef() { }

        public BuildingRef(Building building) {
            position = building.position;
            empire = building.empire.id;
            type = building.type;
            level = building.level;
        }
    }
}