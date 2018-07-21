using Glint;
using Nez.BitmapFonts;

namespace SquareEmpires.Assets {
    public class UiAssets {
        // ReSharper disable once InconsistentNaming
        public BitmapFont AndinaBMFont;

        public void load() {
            AndinaBMFont = GlintCore.contentSource.Load<BitmapFont>("Fonts/bm_andina");
        }
    }
}