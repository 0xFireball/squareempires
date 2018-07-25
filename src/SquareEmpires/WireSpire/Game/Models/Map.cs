using WireSpire.Types;

namespace WireSpire {
    public class Map {
        public enum Terrain {
            UNKNOWN,
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

        public Tile[] tiles;
        public Position size;

        public Tile get(Position pos) {
            return tiles[pos.y * size.x + pos.x];
        }

        public void set(Position pos, Tile tile) {
            tiles[pos.y * size.x + pos.x] = tile;
        }

        public Map(Position size) {
            this.size = size;
            tiles = new Tile[size.x * size.y];
        }
    }
}