using Glint;
using Glint.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez.Textures;

namespace SquareEmpires.Components.UI {
    public class PointerCursor : FufAnimatedSprite<PointerCursor.Animations> {
        public const int curSize = 12;

        public enum Animations {
            Base
        }

        public PointerCursor(int renderLayer) :
            base(GlintCore.contentSource.Load<Texture2D>("Sprites/UI/cursor"), curSize, curSize) {
            animation.renderLayer = renderLayer;
            animation.setLocalOffset(new Vector2(curSize / 2f));
        }
    }
}