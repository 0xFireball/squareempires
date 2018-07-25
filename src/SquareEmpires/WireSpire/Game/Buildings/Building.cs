using WireSpire.Refs;
using WireSpire.Types;

namespace WireSpire {
    public enum BuildingType {
        None,
        Station
    }

    public abstract class Building {
        public Position position;
        public Empire empire;

        public int level = 0;
        public virtual BuildingType type => BuildingType.None;

        public Building(Empire empire, Position position) {
            this.empire = empire;
            this.position = position;
        }
    }
}