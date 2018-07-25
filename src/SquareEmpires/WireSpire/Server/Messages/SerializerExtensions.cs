using Tempest;
using WireSpire.Types;

namespace WireSpire.Server.Messages {
    public static class SerializerExtensions {
        public static void writePosition(this IValueWriter writer, Position pos) {
            writer.WriteInt32(pos.x);
            writer.WriteInt32(pos.y);
        }

        public static Position readPosition(this IValueReader reader) {
            return new Position(reader.ReadInt32(), reader.ReadInt32());
        }
    }
}