using WireSpire.Types;

namespace WireSpire.Entities {
    public abstract class Pawn {
        public enum Type {
            Person,
            Scout
        }

        // - pawn data
        public Position pos;
        public Empire empire;
        public int level = 0;
        public virtual Type type => Type.Person;
        
        // - pawn internal game values
        public virtual int vision => 1;

        public Pawn(Empire empire, Position pos) {
            this.empire = empire;
            this.pos = pos;
        }
    }
}