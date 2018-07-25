using System.Collections.Generic;
using Tempest;
using WireSpire.Refs;
using WireSpire.Server.Mech;
using WireSpire.Types;

namespace WireSpire.Server.Messages {
    public class WorldUpdateMessage : RemoteGameMessage {
        public WorldUpdateMessage() : base(MessageKind.WorldUpdate) { }

        public ObservedWorld world;

        public override void WritePayload(ISerializationContext context, IValueWriter writer) {
            writer.WriteInt32(world.tiles.Count);
            foreach (var (pos, tile) in world.tiles) {
                writer.writePosition(pos);
                writer.writeTile(tile);
            }
            writer.WriteInt32(world.buildings.Count);
            foreach (var building in world.buildings) {
                writer.writeBuilding(building);
            }
            writer.WriteInt32(world.pawns.Count);
            foreach (var pawn in world.pawns) {
                writer.writePawn(pawn);
            }
        }

        public override void ReadPayload(ISerializationContext context, IValueReader reader) {
            world = new ObservedWorld();
            world.tiles = new List<(Position, TileRef)>();
            var tileCount = reader.ReadInt32();
            for (var i = 0; i < tileCount; i++) {
                var pos = reader.readPosition();
                var tile = reader.readTile();
                world.tiles.Add((pos, tile));
            }
            world.buildings = new List<BuildingRef>();
            var buildingCount = reader.ReadInt32();
            for (var i = 0; i < buildingCount; i++) {
                var building = reader.readBuilding();
                world.buildings.Add(building);
            }
            world.pawns = new List<PawnRef>();
            var pawnCount = reader.ReadInt32();
            for (var i = 0; i < pawnCount; i++) {
                var pawn = reader.readPawn();
                world.pawns.Add(pawn);
            }
        }
    }
}