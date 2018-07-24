using System.Threading.Tasks;
using Tempest;
using WireSpire.Server.Messages;

namespace WireSpire.Client {
    public class RemoteGameClient : TempestClient {
        public RemoteGameClient(IClientConnection connection) : base(connection, MessageTypes.Reliable) {
            this.RegisterMessageHandler<EmpireAssignmentMessage>(onEmpireAssigned);
        }

        public Task joinGameAsync(bool ready) {
            var msg = new JoinMessage {ready = ready};
            return Connection.SendAsync(msg);
        }

        private void onEmpireAssigned(MessageEventArgs<EmpireAssignmentMessage> obj) {
            throw new System.NotImplementedException();
        }
    }
}