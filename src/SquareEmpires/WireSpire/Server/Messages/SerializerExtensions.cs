using System.Collections.Generic;
using Tempest;
using WireSpire.Refs;
using WireSpire.Types;

namespace WireSpire.Server.Messages {
    public static class SerializerExtensions {
        public static void writePosition(this IValueWriter writer, Position pos) {
            writer.WriteInt32(pos.x);
            writer.WriteInt32(pos.y);
        }

        public static Position readPosition(this IValueReader reader) {
            return new Position(reader.ReadInt32(), reader.ReadInt32());
        }

        public static void writeBuilding(this IValueWriter writer, BuildingRef building) {
            writer.WriteInt32(building.empire);
            writer.WriteInt32((int) building.type);
            writer.WriteInt32(building.level);
            writer.writePosition(building.pos);
        }

        public static BuildingRef readBuilding(this IValueReader reader) {
            return new BuildingRef {
                empire = reader.ReadInt32(),
                type = (BuildingType) reader.ReadInt32(),
                level = reader.ReadInt32(),
                pos = reader.readPosition()
            };
        }

        public static void writeTile(this IValueWriter writer, TileRef tile) {
            writer.WriteInt32((int) tile.ter);
            writer.WriteInt32(tile.resources.Count);
            foreach (var (resource, amount) in tile.resources) {
                writer.WriteInt32(resource);
                writer.WriteInt64(amount);
            }
        }

        public static TileRef readTile(this IValueReader reader) {
            var tile = new TileRef();
            var resourceCount = reader.ReadInt32();
            tile.resources = new List<(int, long)>(resourceCount);
            for (var i = 0; i < resourceCount; i++) {
                tile.resources.Add((reader.ReadInt32(), reader.ReadInt64()));
            }

            return tile;
        }
    }
}