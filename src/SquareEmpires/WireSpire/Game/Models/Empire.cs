using System.Collections.Generic;
using WireSpire.Entities;

namespace WireSpire {
    public class Empire {
        public string name;
        public int id;

        public ResourceTable resources = new ResourceTable();
        public Station capital;
        public List<Building> buildings = new List<Building>();
        public List<Pawn> pawns = new List<Pawn>();
        
        // economy
        public ResourceTable baseIncome = new ResourceTable();

        public void addBuilding(Building building) {
            building.empire = this;
            buildings.Add(building);
        }

        public void addPawn(Pawn pawn) {
            pawn.empire = this;
            pawns.Add(pawn);
        }
    }
}