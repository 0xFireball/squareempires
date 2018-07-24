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

        public Tile[] tiles;
        public Position size;

        public Tile get(int x, int y) {
            return tiles[y * size.x + x];
        }

        public void set(int x, int y, Tile tile) {
            tiles[y * size.x + x] = tile;
        }

        public Map(Position size) {
            this.size = size;
            tiles = new Tile[size.x * size.y];
        }
    }
}