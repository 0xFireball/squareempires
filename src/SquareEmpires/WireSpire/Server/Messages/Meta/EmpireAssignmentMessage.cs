using System;
using Tempest;

namespace WireSpire.Server.Messages {
    public class EmpireAssignmentMessage : RemoteGameMessage {
        public EmpireAssignmentMessage() : base(MessageKind.EmpireAssignment) { }

        public int empireId;

        public override void WritePayload(ISerializationContext context, IValueWriter writer) {
            writer.WriteInt32(empireId);
        }

        public override void ReadPayload(ISerializationContext context, IValueReader reader) {
            empireId = reader.ReadInt32();
        }
    }
}