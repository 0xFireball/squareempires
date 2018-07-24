using Tempest;

namespace WireSpire.Server {
    public class ConnectedPlayer {
        public int id;
        public int empireId;
        public IConnection connection;

        public ConnectedPlayer(int id, IConnection connection) {
            this.id = id;
            this.connection = connection;
        }
    }
}