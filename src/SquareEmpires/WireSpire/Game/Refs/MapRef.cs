using System.Collections.Generic;
using System.Linq;
using WireSpire.Types;

namespace WireSpire.Refs {
    public class MapRef {
        public Position size;
        public TileRef[] tiles;

        public MapRef(Position size) {
            this.size = size;
            tiles = new TileRef[size.x * size.y];
            for (var i = 0; i < tiles.Length; i++) {
                tiles[i] = new TileRef {ter = Map.Terrain.UNKNOWN};
            }
        }

        public TileRef get(Position pos) {
            return tiles[pos.y * size.x + pos.x];
        }

        public void set(Position pos, TileRef tile) {
            tiles[pos.y * size.x + pos.x] = tile;
        }

        public void step() {
            for (var i = 0; i < tiles.Length; i++) {
                tiles[i].fresh = false;
            }
        }
    }

    public class TileRef {
        public Map.Terrain ter;
        public List<(int, long)> resources;
        public bool fresh = true; // used client-side to check if tiles are updated
        public override string ToString() => ter.ToString();

        public TileRef() { }

        public TileRef(Map.Tile tile) {
            ter = tile.terrain;
            resources = tile.resources?.table.Select(x => (x.Key, x.Value)).ToList() ?? new List<(int, long)>();
        }
    }
}