using WireSpire.Types;

namespace WireSpire {
    public class Map {
        public enum Terrain {
            LAND,
            WATER,
        }

        public class Tile {
            public Terrain terrain;
            public ResourceTable resources;

            public Tile(Terrain terrain, ResourceTable resources = null) {
                this.terrain = terrain;
                this.resources = resources;
            }
        }

        public Tile[,] tiles;
        public Position size;

        public Map(Position size) {
            this.size = size;
            tiles = new Tile[size.x, size.y];
        }
    }
}