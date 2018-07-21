using System.Collections.Generic;
using System.Linq;
using Glint;
using Glint.Sprites;
using Microsoft.Xna.Framework.Graphics;
using Nez.Sprites;
using Nez.Textures;

namespace SquareEmpires.Components.UI {
    class TitleCover : FufSprite {
        public TitleCover() : base(GlintCore.contentSource.Load<Texture2D>("Sprites/Anim/title_cover")) { }
    }
}