using System;
using System.Threading;
using Glint.UI;
using Glint.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nez;
using SquareEmpires.Assets;
using SquareEmpires.Components.UI;
using SquareEmpires.Configuration;
using SquareEmpires.Scenes.Base;
using SquareEmpires.Scenes.Menu;
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
        private ServerInformation serverInformation;
        private GameServer server;
        private Thread serverThread;
        private RemoteGameClient client;

        // -- constants
        public const int DEFAULT_GAME_PORT = 14834;

        public class ServerInformation {
            public string ip;
            public int port;
        }

        public GamePlayScene(ServerInformation serverInformation) {
            this.serverInformation = serverInformation;
        }

        public override void initialize() {
            base.initialize();

            clearColor = Color.DarkSlateBlue;

            // data
            _gameContext = Core.services.GetService<GameContext>();
            var uiAssets = Core.services.GetService<UiAssets>();

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

            // add loading text
            var loadingUi = createEntity("loading");
            loadingUi.addComponent(
                new Text(uiAssets.AndinaBMFont, "loading\n. . .", new Vector2(resolution.X / 2, resolution.Y - 100),
                    Color.WhiteSmoke).setHorizontalAlign(HorizontalAlign.Center)).transform.setLocalScale(2);
        }

        public override void onStart() {
            base.onStart();

            if (serverInformation == null) {
                // we need to run a server
                var localServerPort = DEFAULT_GAME_PORT;
                server = new GameServer(new NetworkConnectionProvider(RemoteGameProtocol.instance,
                    new Target(Target.AnyIP, localServerPort), GameServer.MAX_CONNECTIONS));
                server.initializeSimulation();
                // run the server on a server thread
                serverThread = new Thread(server.Start);
                serverThread.Start();
                serverInformation = new ServerInformation {ip = Target.LoopbackIP, port = localServerPort};
            }

            // run a client and connect
            // TODO: locally generate and store RSA client key
            client = new RemoteGameClient(new NetworkClientConnection(RemoteGameProtocol.instance));
            var clientConnectTask = client.ConnectAsync(new Target(serverInformation.ip, serverInformation.port));
            clientConnectTask.ContinueWith(x => client.joinGameAsync(true));
            client.empireAssigned = setupOnConnected;
        }

        private void setupOnConnected() {
            // TODO: set up rendering and stuff
            clearColor = new Color(230, 177, 213);
            findEntity("loading").destroy();
            // set up the board
        }

        public override void unload() {
            base.unload();

            // stop the server
            if (server != null && server.IsRunning) {
                server.Stop();
            }
        }

        public override void update() {
            base.update();

            if (Input.isKeyPressed(Keys.Escape)) {
                // end this scene
                switchSceneFade<MenuScene>(0.1f);
            }

            if (client.myEmpire > 0) {
                // TODO: ??? hmmm
            }

            // TODO: other stuff and things
#if DEBUG
            if (Input.isKeyPressed(Keys.OemCloseBrackets)) {
                Core.debugRenderEnabled = !Core.debugRenderEnabled;
            }
#endif
        }
    }
}