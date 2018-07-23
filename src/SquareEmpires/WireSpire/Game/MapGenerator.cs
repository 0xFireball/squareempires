using WireSpire.Types;

namespace WireSpire {
    public class MapGenerator {
        public Map generate(Position size) {
            // TODO: make this an actual map generator
            var map = new Map(size);
            for (var i = 0; i < map.size.x; i++) {
                for (var j = 0; j < map.size.y; j++) {
                    map.tiles[i, j] = new Map.Tile(Map.Terrain.LAND);
                }
            }

            return map;
        }
    }
}