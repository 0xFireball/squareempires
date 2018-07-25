using WireSpire.Types;

namespace WireSpire.Refs {
    public enum BuildingRefType {
        None,
        Station
    }

    public class BuildingRef {
        public Position position;
        public int empire;
        public BuildingRefType type;
        public int level = 0;
    }
}