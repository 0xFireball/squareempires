using System;
using System.Collections.Generic;
using System.Linq;
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
        // -- subtextures
        //     > tile
        private Subtexture unknownTileSubtex;
        private Subtexture baseTileSubtex;
        private Subtexture propertyTileSubtex;
        private Subtexture highlightedTileSubtex;

        //     > tile_display
        private Subtexture tileDisplayActiveSubtex;
        private Subtexture tileDisplayTargetSubtex;

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
        public const int TILE_TEXTURE_SIZE = 32;

        // -- callbacks
        public Action<PawnRef, Position> pawnMove;

        private ThingRef selectedThing;

        public GameBoard(RemoteGameState gameState) {
            this.gameState = gameState;

            var tileTexture = GlintCore.contentSource.Load<Texture2D>("Sprites/Game/tile");
            unknownTileSubtex = new Subtexture(tileTexture, new Rectangle(0, 0, TILE_TEXTURE_SIZE, TILE_TEXTURE_SIZE));
            baseTileSubtex = new Subtexture(tileTexture,
                new Rectangle(TILE_TEXTURE_SIZE, 0, TILE_TEXTURE_SIZE, TILE_TEXTURE_SIZE));
            propertyTileSubtex = new Subtexture(tileTexture,
                new Rectangle(0, TILE_TEXTURE_SIZE, TILE_TEXTURE_SIZE, TILE_TEXTURE_SIZE));
            highlightedTileSubtex = new Subtexture(tileTexture,
                new Rectangle(TILE_TEXTURE_SIZE, TILE_TEXTURE_SIZE, TILE_TEXTURE_SIZE, TILE_TEXTURE_SIZE));

            var tileDisplayTexture = GlintCore.contentSource.Load<Texture2D>("Sprites/Game/tile_display");
            tileDisplayActiveSubtex = new Subtexture(tileDisplayTexture,
                new Rectangle(0, 0, TILE_TEXTURE_SIZE, TILE_TEXTURE_SIZE));
            tileDisplayTargetSubtex = new Subtexture(tileDisplayTexture,
                new Rectangle(TILE_TEXTURE_SIZE, 0, TILE_TEXTURE_SIZE, TILE_TEXTURE_SIZE));

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

        private bool inBoard(Position pos) {
            return pos.x >= 0 && pos.y >= 0 && pos.x < map.size.x && pos.y < map.size.y;
        }

        public override void render(Graphics graphics, Camera camera) {
            Vector2 vpos(Position pos) {
                return entity.transform.position + localOffset + tilePosition(pos);
            }

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

            // draw selection info
            if (selectedThing != null) {
                var selectionColor = Color.LightSteelBlue;
                graphics.batcher.draw(tileDisplayActiveSubtex, vpos(selectedThing.pos), selectionColor, rotation: 0f,
                    origin: Vector2.Zero,
                    scale: Vector2.One, effects: SpriteEffects.None, layerDepth: 1f);
                if (selectedThing is PawnRef selectedPawn) {
                    // draw available movements
                    var moveRadius = Pawn.moveSpeed[selectedPawn.type];
                    for (var i = -moveRadius; i <= moveRadius; i++) {
                        for (var j = -moveRadius; j <= moveRadius; j++) {
                            if (i == 0 && j == 0) continue;
                            var endPos = selectedThing.pos + new Position(i, j);
                            if (!inBoard(endPos)) continue;
                            graphics.batcher.draw(tileDisplayTargetSubtex, vpos(endPos), selectionColor, rotation: 0f,
                                origin: Vector2.Zero,
                                scale: Vector2.One, effects: SpriteEffects.None, layerDepth: 0f);
                        }
                    }
                }
            }

            // draw buildings
            if (gameState.buildings != null) {
                foreach (var building in gameState.buildings) {
                    var texture = pickTexture(building);
                    graphics.batcher.draw(texture, vpos(building.pos), Color.White);
                    graphics.batcher.draw(propertyTileSubtex, vpos(building.pos), pickEmpireColor(building.empire),
                        rotation: 0f, origin: Vector2.Zero,
                        scale: Vector2.One, effects: SpriteEffects.None, layerDepth: 1f);
                }
            }

            // draw pawns
            // TODO: highlight our pawns with available actions
            if (gameState.pawns != null) {
                foreach (var pawn in gameState.pawns) {
                    var texture = pickTexture(pawn);
                    var pawnCol = pickEmpireColor(pawn.empire);
                    // if move is available, fuzz outline
                    if (pawn.lastMove < gameState.time) {
                        graphics.batcher.draw(texture, vpos(pawn.pos) + new Vector2(TILE_DRAW_SIZE / 2f), pawnCol.multiply(new Color(200, 200, 200, 100)),
                            rotation: 0f, origin: texture.origin,
                            scale: new Vector2(1.4f), effects: SpriteEffects.None, layerDepth: 0.1f);
                    }
                    graphics.batcher.draw(texture, vpos(pawn.pos), pawnCol);
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

        public void update() {
            Position getMouseTile() {
                // calculate the selection tilepos
                var selectionPos =
                    Vector2Ext.transform(Input.mousePosition, entity.scene.camera.inverseTransformMatrix);
                var relativeSelectionPos = selectionPos - (entity.transform.position + localOffset);
                var mouseTilePos = new Position((int) relativeSelectionPos.X / TILE_DRAW_SIZE,
                    (int) relativeSelectionPos.Y / TILE_DRAW_SIZE);
                return mouseTilePos;
            }

            var selectionTilePos = getMouseTile();
            if (Input.leftMouseButtonPressed) {
                // check if there's a selectable item on that tile
                var therePawn = gameState.pawns.FirstOrDefault(x => x.pos.equalTo(selectionTilePos));
                if (therePawn != null) {
                    if (therePawn.lastMove < gameState.time) {
                        selectedThing = therePawn;
                    }
                }
            }

            if (Input.rightMouseButtonPressed) {
                // check if we had a selection and apply it
                if (selectedThing != null) {
                    if (selectedThing is PawnRef pawn) {
                        // TODO: queue sending move message
                        pawnMove?.Invoke(pawn, selectionTilePos);
                        pawn.lastMove = gameState.time;
                        selectedThing = null; // deselect
                    }
                }
            }
        }
    }
}