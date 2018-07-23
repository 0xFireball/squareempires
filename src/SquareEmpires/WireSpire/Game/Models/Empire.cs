using System.Collections.Generic;

namespace WireSpire {
    public class Empire {
        public string name;

        public ResourceTable resources = new ResourceTable();
        public Station capital;
        public List<Building> buildings = new List<Building>();
        
        // economy
        public ResourceTable baseIncome = new ResourceTable();

        public void addBuilding(Building building) {
            building.empire = this;
            buildings.Add(building);
        }
    }
}