using System.Collections.Generic;
using Tempest;
using WireSpire.Refs;
using WireSpire.Types;

namespace WireSpire.Server.Messages {
    public class WorldUpdateMessage : RemoteGameMessage {
        public WorldUpdateMessage() : base(MessageKind.WorldUpdate) { }

        public List<(Position, TileRef)> tiles;

        public override void WritePayload(ISerializationContext context, IValueWriter writer) {
            writer.WriteInt32(tiles.Count);
            foreach (var (pos, tile) in tiles) {
                writer.writePosition(pos);
                writer.writeTile(tile);
            }
        }

        public override void ReadPayload(ISerializationContext context, IValueReader reader) {
            tiles = new List<(Position, TileRef)>();
            var tileCount = reader.ReadInt32();
            for (var i = 0; i < tileCount; i++) {
                var pos = reader.readPosition();
                var tile = reader.readTile();
                tiles.Add((pos, tile));
            }
        }
    }
}