using System.Collections.Generic;

namespace WireSpire {
    public class Resource {
        public const int MONEY = 0;
        public const int METAL = 1;
        public const int INFLUENCE = 2;
    }

    public class ResourceTable {
        public long getResource(int id) {
            if (table.ContainsKey(id)) return table[id];
            return 0;
        }

        public void addResource(int id, long amount) {
            table[id] = getResource(id) + amount;
        }

        public Dictionary<int, long> table = new Dictionary<int, long>();
    }
}