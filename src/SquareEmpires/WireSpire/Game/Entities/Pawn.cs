using WireSpire.Types;

namespace WireSpire.Entities {
    public abstract class Pawn {
        public enum Type {
            Warrior,
            Scout,
            Archer,
            Swordsman,
            Knight,
            Philosopher,
            General,
            Trebuchet,
            Cannon
        }

        // - pawn data
        public Position pos;
        public Empire empire;
        public int level = 0;
        public virtual Type type => Type.Warrior;
        
        // - pawn internal game values
        public virtual int vision => 1;

        public Pawn(Empire empire, Position pos) {
            this.empire = empire;
            this.pos = pos;
        }
    }
}