using System;
using System.IO;
using SquareEmpires;
using SquareEmpires.Configuration;
using Newtonsoft.Json;

namespace SquareEmpiresDk {
    public static class Program {
        public const string CONFIG_FILE = "game_config.json";

        [STAThread]
        static void Main() {
            GameConfiguration config = null;
            // load config file
            if (File.Exists(CONFIG_FILE)) {
                config = JsonConvert.DeserializeObject<GameConfiguration>(File.ReadAllText(CONFIG_FILE));
            } else {
                config = new GameConfiguration();
                File.WriteAllText(CONFIG_FILE, JsonConvert.SerializeObject(config, Formatting.Indented));
            }

            config.bind(CONFIG_FILE);
            var context = new GameContext(config);

            using (var game = new NGame(context)) {
                game.Run();
            }
        }
    }
}