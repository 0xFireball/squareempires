using System.Collections.Generic;

namespace WireSpire {
    public class World {
        public Map map;
        public List<Empire> empires;

        public World(Map map, List<Empire> empires) {
            this.map = map;
            this.empires = empires;
        }
    }
}