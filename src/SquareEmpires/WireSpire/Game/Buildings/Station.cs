using WireSpire.Types;

namespace WireSpire {
    public class Station : Building {
        public enum Level {
            Frame = 0,
            Outpost = 1,
            Holding = 2,
            Hall = 3,
            Fortress = 4,
            Bastion = 5,
            Administration = 6,
            Citadel = 7,
            Glitterite = 8,
        }

        public Station(Empire empire, Position position, Level level = Level.Outpost) : base(empire, position) { }
    }
}