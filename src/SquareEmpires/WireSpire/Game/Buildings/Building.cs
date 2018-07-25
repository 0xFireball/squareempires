using WireSpire.Types;

namespace WireSpire {
    public abstract class Building {
        public enum Type {
            None,
            Station
        }
        
        public Position pos;
        public Empire empire;
        public int level = 0;
        public virtual Type type => Type.None;

        public Building(Empire empire, Position pos) {
            this.empire = empire;
            this.pos = pos;
        }
    }
}