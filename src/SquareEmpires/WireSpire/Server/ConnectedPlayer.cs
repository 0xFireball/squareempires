using System.Net.Sockets;

namespace WireSpire.Server {
    public class ConnectedPlayer {
        public int id;
        public TcpClient sock;
        public byte[] buffer = new byte[BUFFER_SIZE];

        public const int BUFFER_SIZE = 16 * 1024;

        public ConnectedPlayer(int id, TcpClient sock) {
            this.id = id;
            this.sock = sock;
        }
    }
}