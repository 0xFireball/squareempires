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
    }

    public class TileRef {
        public Map.Terrain ter;
        public List<(int, long)> resources;
        public override string ToString() => ter.ToString();

        public TileRef() { }

        public TileRef(Map.Tile tile) {
            ter = tile.terrain;
            resources = tile.resources.table.Select(x => (x.Key, x.Value)).ToList();
        }
    }
}