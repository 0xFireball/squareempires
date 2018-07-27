using System.Collections.Generic;
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
        public int lastMove = -1; // never
        
        // - pawn internal game values
        public static Dictionary<Type, int> vision = new Dictionary<Type, int>() {
            [Type.Warrior] = 1,
            [Type.Scout] = 3,
            [Type.Archer] = 2,
            [Type.Swordsman] = 1,
            [Type.Knight] = 1,
        };
        public static Dictionary<Type, int> moveSpeed = new Dictionary<Type, int>() {
            [Type.Warrior] = 1,
            [Type.Scout] = 2,
            [Type.Archer] = 1,
            [Type.Swordsman] = 1,
            [Type.Knight] = 3,
        };

        public Pawn(Empire empire, Position pos) {
            this.empire = empire;
            this.pos = pos;
        }

        public void move() {
            
        }
    }
}