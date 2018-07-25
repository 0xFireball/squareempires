using WireSpire.Types;

namespace WireSpire {
    public abstract class Building {
        public Position position;
        public Empire empire;

        public Building(Empire empire, Position position) {
            this.empire = empire;
            this.position = position;
        }
    }
}