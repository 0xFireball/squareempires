using System.Collections.Generic;
using Glint;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Textures;
using SquareEmpires.Game;
using WireSpire;
using WireSpire.Entities;
using WireSpire.Refs;
using WireSpire.Types;

namespace SquareEmpires.Components.Board {
    public class GameBoard : RenderableComponent, IUpdatable {
        private Subtexture unknownTileSubtex;
        private Subtexture baseTileSubtex;
        private Subtexture propertyTileSubtex;

        private List<Subtexture> stationSubtexes;
        private List<Subtexture> pawnSubtexes;

        private RemoteGameState gameState;
        public MapRef map => gameState.map;

        public List<Color> empireColors = new List<Color>() {
            Color.DeepPink,
            Color.Blue,
            Color.DarkGreen,
            Color.DarkOrange,
            Color.Purple,
            Color.Yellow
        };

        public const int TILE_DRAW_SIZE = 32;
        private const int TILE_TEXTURE_SIZE = 32;

        public GameBoard(RemoteGameState gameState) {
            this.gameState = gameState;

            var tileTexture = GlintCore.contentSource.Load<Texture2D>("Sprites/Game/tile");
            unknownTileSubtex = new Subtexture(tileTexture, new Rectangle(0, 0, TILE_TEXTURE_SIZE, TILE_TEXTURE_SIZE));
            baseTileSubtex = new Subtexture(tileTexture,
                new Rectangle(TILE_TEXTURE_SIZE, 0, TILE_TEXTURE_SIZE, TILE_TEXTURE_SIZE));
            propertyTileSubtex = new Subtexture(tileTexture,
                new Rectangle(TILE_TEXTURE_SIZE * 2, 0, TILE_TEXTURE_SIZE, TILE_TEXTURE_SIZE));

            var stationsTexture = GlintCore.contentSource.Load<Texture2D>("Sprites/Game/station");
            stationSubtexes = Subtexture.subtexturesFromAtlas(stationsTexture, TILE_TEXTURE_SIZE, TILE_TEXTURE_SIZE);

            var pawnsTexture = GlintCore.contentSource.Load<Texture2D>("Sprites/Game/pawns");
            pawnSubtexes = Subtexture.subtexturesFromAtlas(pawnsTexture, TILE_TEXTURE_SIZE, TILE_TEXTURE_SIZE);
        }

        public override RectangleF bounds {
            get {
                if (_areBoundsDirty) {
                    _bounds.calculateBounds(entity.transform.position, _localOffset, Vector2.Zero,
                        entity.transform.scale,
                        entity.transform.rotation, map.size.x * TILE_DRAW_SIZE, map.size.y * TILE_DRAW_SIZE);
                    _areBoundsDirty = false;
                }

                return _bounds;
            }
        }

        private Vector2 tilePosition(Position pos) {
            return new Vector2(pos.x * TILE_DRAW_SIZE, pos.y * TILE_DRAW_SIZE);
        }

        public override void render(Graphics graphics, Camera camera) {
            // draw tiles
            graphics.batcher.drawRect(
                new Rectangle((entity.transform.position + localOffset).roundToPoint(),
                    new Point(map.size.x * TILE_DRAW_SIZE, map.size.y * TILE_DRAW_SIZE)), new Color(230, 177, 213));
            if (map != null) {
                for (var j = 0; j < map.size.y; j++) {
                    for (var i = 0; i < map.size.x; i++) {
                        var tile = map.tiles[j * map.size.x + i];
                        var tileTex = pickTexture(tile.ter);
                        var tileCol = Color.White;
                        if (tile.ter != Map.Terrain.UNKNOWN && !tile.fresh) {
                            tileCol = Color.Gray;
                        }

                        graphics.batcher.draw(tileTex,
                            entity.transform.position + localOffset + tilePosition(new Position(i, j)),
                            tileCol);
                    }
                }
            }

            if (gameState.buildings != null) {
                foreach (var building in gameState.buildings) {
                    var texture = pickTexture(building);
                    var vpos = entity.transform.position + localOffset + tilePosition(building.pos);
                    graphics.batcher.draw(texture, vpos, Color.White);
                    graphics.batcher.draw(propertyTileSubtex, vpos, pickEmpireColor(building.empire));
                }
            }

            if (gameState.pawns != null) {
                foreach (var pawn in gameState.pawns) {
                    var texture = pickTexture(pawn);
                    var vpos = entity.transform.position + localOffset + tilePosition(pawn.pos);
                    var pawnCol = pickEmpireColor(pawn.empire);
                    graphics.batcher.draw(texture, vpos, pawnCol);
                }
            }
        }

        private Subtexture pickTexture(Map.Terrain tileType) {
            switch (tileType) {
                case Map.Terrain.UNKNOWN:
                    return unknownTileSubtex;
                    break;
                case Map.Terrain.LAND:
                    return baseTileSubtex;
                    break;
                default:
                    return null;
            }
        }

        private Subtexture pickTexture(BuildingRef buildingRef) {
            switch (buildingRef.type) {
                case Building.Type.Station:
                    return stationSubtexes[buildingRef.level - 1];
                default:
                    return null;
            }
        }

        private Subtexture pickTexture(PawnRef pawnRef) {
            return pawnSubtexes[(int) pawnRef.type];
        }

        private Color pickEmpireColor(int empireId) {
            return empireColors[empireId % empireColors.Count];
        }

        public void update() { }
    }
}