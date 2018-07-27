using System.Collections.Generic;
using WireSpire.Types;

namespace WireSpire {
    public class World {
        public Map map;
        public List<Empire> empires;

        public World(Map map, List<Empire> empires) {
            this.map = map;
            this.empires = empires;
        }

        public bool inWorld(Position pos) {
            return pos.x >= 0 && pos.y >= 0 && pos.x < map.size.x && pos.y < map.size.y;
        }
    }
}