using WireSpire.Types;

namespace SquareEmpires.Game {
    public class MapRef {
        public Position size;
        public TileRef[] tiles;

        public MapRef(Position size) {
            this.size = size;
            tiles = new TileRef[size.x * size.y];
            for (var i = 0; i < tiles.Length; i++) {
                tiles[i] = new TileRef {type = TileType.Unknown};
            }
        }
    }

    public enum TileType {
        Unknown,
        Land
    }

    public class TileRef {
        public TileType type;
        public override string ToString() => type.ToString();
    }
}