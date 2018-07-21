using Glint;
using Glint.Sprites;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nez.Textures;

namespace SquareEmpires.Components.UI {
    class IndicatorBar : FufSprite {
        public int width;
        public int height;

        public IndicatorBar(int width, int height) : base(GlintCore.contentSource.Load<Texture2D>("Sprites/UI/indicator_bar")) {
            this.width = width;
            this.height = height;
        }

        public override void onAddedToEntity() {
            base.onAddedToEntity();

            sprite.setSubtexture(new Subtexture(texture, new Rectangle(0, 0, width, height), Vector2.Zero));
        }

        public void setValue(float value) {
            sprite.setSubtexture(new Subtexture(texture, new Rectangle(0, 0, (int) (value * width), height), Vector2.Zero));
        }
    }
}