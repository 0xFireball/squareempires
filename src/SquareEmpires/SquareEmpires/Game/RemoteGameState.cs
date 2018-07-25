using System.Collections.Generic;
using WireSpire.Refs;

namespace SquareEmpires.Game {
    public class RemoteGameState {
        public int empireId;
        public List<EmpireRef> empires;
        public List<BuildingRef> buildings;
        public List<PawnRef> pawns;
        public MapRef map;
    }
}