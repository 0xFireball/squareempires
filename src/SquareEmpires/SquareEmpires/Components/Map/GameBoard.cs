using System.Net.NetworkInformation;
using Glint;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez;
using Nez.Textures;
using SquareEmpires.Game;

namespace SquareEmpires.Components.Map {
    public class GameBoard : RenderableComponent, IUpdatable {
        private Subtexture unknownTileSubtex;
        private Subtexture baseTileSubtex;
        private Subtexture propertyTileSubtex;

        public MapRef map;

        public const int TILE_DRAW_SIZE = 32;

        public GameBoard(MapRef map) {
            this.map = map;

            var tileTexture = GlintCore.contentSource.Load<Texture2D>("Sprites/Game/tile");
            unknownTileSubtex = new Subtexture(tileTexture, new Rectangle(0, 0, 32, 32));
            baseTileSubtex = new Subtexture(tileTexture, new Rectangle(32, 0, 32, 32));
            propertyTileSubtex = new Subtexture(tileTexture, new Rectangle(64, 0, 32, 32));
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

        public override void render(Graphics graphics, Camera camera) {
            // draw tiles
            graphics.batcher.drawRect(
                new Rectangle((entity.transform.position + localOffset).roundToPoint(),
                    new Point(map.size.x * TILE_DRAW_SIZE, map.size.y * TILE_DRAW_SIZE)), new Color(230, 177, 213));
            if (map != null) {
                for (var j = 0; j < map.size.y; j++) {
                    for (var i = 0; i < map.size.x; i++) {
                        var tile = map.tiles[j * map.size.x + i];
                        var rX = i * TILE_DRAW_SIZE;
                        var rY = j * TILE_DRAW_SIZE;
                        graphics.batcher.draw(pickTexture(tile.type),
                            entity.transform.position + localOffset + new Vector2(rX, rY),
                            Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0f);
                    }
                }
            }
        }

        private Subtexture pickTexture(TileType tileType) {
            switch (tileType) {
                case TileType.Unknown:
                    return unknownTileSubtex;
                    break;
                case TileType.Land:
                    return baseTileSubtex;
                    break;
                default:
                    return null;
            }
        }

        public void update() { }
    }
}