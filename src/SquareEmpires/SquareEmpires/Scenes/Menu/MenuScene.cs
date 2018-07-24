using Glint.UI;
using SquareEmpires.Assets;
using SquareEmpires.Scenes.Base;
using SquareEmpires.Scenes.Game;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using Nez.Tweens;

namespace SquareEmpires.Scenes.Menu {
    public class MenuScene : BaseGameScene {
        public override void initialize() {
            base.initialize();

            clearColor = new Color(47, 47, 47);

            var uiAssets = Core.services.GetService<UiAssets>();
            var borderPadding = 20;

            // title picture
            var titleCoverEntity = createEntity("intro_cover");
            titleCoverEntity.position = (resolution.ToVector2() / 2) * new Vector2(1, 0.9f);
            titleCoverEntity.localScale = new Vector2(6);
            var titleCover = titleCoverEntity.addComponent<Components.UI.TitleCover>();
            titleCover.sprite.layerDepth = 1;

            var petaphaserTextCom =
                new TextComposer(StringResources.DEVELOPER_NAME, uiAssets.AndinaBMFont, 1f)
                    .attach(this,
                        resolution.ToVector2(), Color.WhiteSmoke, "petaphaser_t");
            petaphaserTextCom.updateOffsets(new Vector2(-(petaphaserTextCom.TextComponent.width + borderPadding),
                -(petaphaserTextCom.TextComponent.height / 2 + borderPadding)));

            petaphaserTextCom.TextComponent
                .tweenColorTo(new Color(255, 15, 127), 0.6f)
                .setEaseType(EaseType.QuadInOut)
                .setLoops(LoopType.PingPong, 96, 0.2f)
                .start();

            var pressToPlayTextCom = new TextComposer("press 'E'", uiAssets.AndinaBMFont, 0.5f)
                .attach(this,
                    new Vector2(20f, resolution.Y - 20f),
                    Color.WhiteSmoke, "playText");
        }

        public override void update() {
            base.update();

            if (Input.isKeyDown(Keys.E)) {
                var text = findEntity("playText").getComponent<Text>();
                text.tweenColorTo(Color.Gold, 0.1f)
                    .setCompletionHandler(_ => switchSceneFade(new GamePlayScene(serverInformation: null), 0.6f))
                    .setNextTween(text.tweenColorTo(Color.Gray, 0.6f).setEaseType(EaseType.QuadOut))
                    .start();
            }

            if (Input.isKeyDown(Keys.R)) {
                var text = findEntity("playText").getComponent<Text>();
                text.tweenColorTo(Color.Gold, 0.1f)
                    .setCompletionHandler(_ =>
                        switchSceneFade(
                            new GamePlayScene(serverInformation: new GamePlayScene.ServerInformation {
                                ip = "127.0.0.1",
                                port = GamePlayScene.DEFAULT_GAME_PORT
                            }), 0.6f))
                    .setNextTween(text.tweenColorTo(Color.Gray, 0.6f).setEaseType(EaseType.QuadOut))
                    .start();
            }
        }
    }
}