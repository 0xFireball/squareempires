using Glint.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using SquareEmpires.Components.UI;
using SquareEmpires.Configuration;
using SquareEmpires.Scenes.Base;

namespace SquareEmpires.Scenes.Game {
    public class GamePlayScene : BaseGameScene {
        private GameContext _gameContext;

        public const int renderlayer_backdrop = 65535;
        public const int renderlayer_ui_overlay = 1 << 30;
        private const int renderlayer_cursor_overlay = 1 << 31;


        public override void initialize() {
            base.initialize();

            clearColor = new Color(230, 177, 213);

            // data
            _gameContext = Core.services.GetService<GameContext>();

            // Hide system cursor
            Core.instance.IsMouseVisible = false;

            // add fixed renderer
            var fixedRenderer =
                addRenderer(new ScreenSpaceRenderer(1023, renderlayer_ui_overlay, renderlayer_cursor_overlay));
            fixedRenderer.shouldDebugRender = false;

            // ...and add custom cursor
            var targetCursor = createEntity("cursor");
            var cursorComponent = targetCursor.addComponent(new PointerCursor(renderlayer_cursor_overlay));
            cursorComponent.sprite.renderLayer = renderlayer_cursor_overlay;
            targetCursor.addComponent<MouseFollow>();
        }

        public override void update() {
            base.update();
            // TODO: other stuff and things

#if DEBUG
            if (Input.isKeyPressed(Keys.OemCloseBrackets)) {
                Core.debugRenderEnabled = !Core.debugRenderEnabled;
            }
#endif
        }
    }
}