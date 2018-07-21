using System.Drawing;
using Glint;
using SquareEmpires.Assets;
using SquareEmpires.Configuration;
using SquareEmpires.Scenes.Menu;
using Nez;

namespace SquareEmpires {
    public class NGame : GlintCore {
        public const string GAME_TITLE = "SquareEmpires";
        public const string GAME_VERSION = "0.0.1-dev";
        
        private readonly GameContext _gameContext;

        public Point gameResolution = new Point(480, 270);

        public NGame(GameContext context) : base(width: 960, height: 540, windowTitle: GAME_TITLE) {
            _gameContext = context;
        }

        protected override void Initialize() {
            base.Initialize();

            Window.Title = GAME_TITLE;
            Window.AllowUserResizing = false;

            // Register context service
            services.AddService(typeof(GameContext), _gameContext);

            var uiAssets = new UiAssets();
            uiAssets.load();
            services.AddService(uiAssets);

            var resolutionPolicy = Scene.SceneResolutionPolicy.ShowAllPixelPerfect;
            if (_gameContext.configuration.graphics.scaleMode ==
                GameConfiguration.GraphicsConfiguration.ScaleMode.Stretch) {
                resolutionPolicy = Scene.SceneResolutionPolicy.BestFit;
            }

            Scene.setDefaultDesignResolution(gameResolution.X, gameResolution.Y, resolutionPolicy);
            
            // Fixed timestep for physics updates
            IsFixedTimeStep = true;

            scene = new IntroScene();
        }
    }
}