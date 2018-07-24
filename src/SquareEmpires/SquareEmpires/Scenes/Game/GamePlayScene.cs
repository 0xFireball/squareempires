using System.Threading;
using Glint.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using SquareEmpires.Components.UI;
using SquareEmpires.Configuration;
using SquareEmpires.Scenes.Base;
using Tempest;
using Tempest.Providers.Network;
using WireSpire.Client;
using WireSpire.Server;

namespace SquareEmpires.Scenes.Game {
    public class GamePlayScene : BaseGameScene {
        // -- rendering
        public const int renderlayer_backdrop = 65535;
        public const int renderlayer_ui_overlay = 1 << 30;
        private const int renderlayer_cursor_overlay = 1 << 31;

        // -- state
        private GameContext _gameContext;

        // -- game
        private GameServer server;
        private Thread serverThread;
        private RemoteGameClient client;

        // -- constants
        public const int DEFAULT_GAME_PORT = 14834;

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

        public override void onStart() {
            base.onStart();

            // we need to run a server
            var localServerPort = DEFAULT_GAME_PORT;
            server = new GameServer(new NetworkConnectionProvider(RemoteGameProtocol.instance,
                new Target(Target.AnyIP, localServerPort), GameServer.MAX_CONNECTIONS));
            server.initializeSimulation();
            // run the server on a server thread
            serverThread = new Thread(server.Start);
            
            // run a client and connect
            client = new RemoteGameClient(new NetworkClientConnection(RemoteGameProtocol.instance, new RSAAsymmetricKey()));
        }

        public override void unload() {
            base.unload();
            
            // stop the server
            server.Stop();
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