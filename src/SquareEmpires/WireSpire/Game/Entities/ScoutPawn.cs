using WireSpire.Types;

namespace WireSpire.Entities {
    public class ScoutPawn : Pawn {
        public ScoutPawn(Empire empire, Position pos) : base(empire, pos) { }

        public override Type type => Type.Scout;
    }
}