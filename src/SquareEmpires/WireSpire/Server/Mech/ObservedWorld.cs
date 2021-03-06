﻿using System.Collections.Generic;
using System.Linq;
using WireSpire.Entities;
using WireSpire.Refs;
using WireSpire.Types;

namespace WireSpire.Server.Mech {
    public class ObservedWorld {
        private World world;
        public readonly Empire empire;

        public List<(Position, TileRef)> tiles;
        public List<BuildingRef> buildings;
        public List<PawnRef> pawns;
        public int time;

        private HashSet<Position> seenTiles;

        public ObservedWorld() { }

        public ObservedWorld(World world, Empire empire, int time) {
            this.world = world;
            this.empire = empire;
            this.time = time;
        }

        public void see() {
            // add my buildings and my pawns
            buildings = empire.buildings.Select(x => new BuildingRef(x)).ToList();
            pawns = empire.pawns.Select(x => new PawnRef(x)).ToList();

            // see the world and populate the observed stuff
            seenTiles = new HashSet<Position>();

            // TODO: better sight radius options
            foreach (var building in empire.buildings.Where(x => x.type == Building.Type.Station)) {
                seeAround(building.pos, 1);
            }

            foreach (var pawn in empire.pawns) {
                seeAround(pawn.pos, Pawn.vision[pawn.type]);
            }

            // convert vision to tiles and entities
            tiles = new List<(Position, TileRef)>();
            foreach (var visiblePosition in seenTiles) {
                var mapTile = new TileRef(world.map.get(visiblePosition));
                tiles.Add((visiblePosition, mapTile));
            }

            // TODO: see other entities
        }

        private void seeAround(Position pos, int amount) {
            if (!world.inWorld(pos)) return;
            if (!seenTiles.Contains(pos)) seenTiles.Add(pos);
            if (amount > 0) {
                amount--;
                for (var dx = -1; dx <= 1; dx++) {
                    for (var dy = -1; dy <= 1; dy++) {
                        if (dx == 0 && dy == 0) continue;
                        seeAround(pos - new Position(dx, dy), amount);
                    }
                }
            }
        }
    }
}