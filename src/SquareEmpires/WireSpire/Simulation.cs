using System.Collections.Generic;
using WireSpire.Types;

namespace WireSpire {
    public class Simulation {
        private readonly int empireCount;
        private readonly Position mapSize;

        public World world;
        public List<Empire> empires = new List<Empire>();
        public int turn;

        public Simulation(int empireCount, Position mapSize) {
            this.empireCount = empireCount;
            this.mapSize = mapSize;
        }

        public void initialize() {
            var mapGenerator = new MapGenerator();
            var map = mapGenerator.generate(mapSize);

            var empireGenerator = new EmpireGenerator();
            for (var i = 0; i < empireCount; i++) {
                var empire = empireGenerator.createEmpire(EmpireGenerator.EmpireTemplates.emergent);
                empires.Add(empire);
            }

            empireGenerator.placeEmpires(empires, map);

            world = new World(map, empires);
        }

        /// <summary>
        /// step the simulation one turn (for a single empire)
        /// </summary>
        public void step() {
            var empire = empires[turn];
            // add all resources
            foreach (var resourceRow in empire.baseIncome.table) {
                empire.resources.addResource(resourceRow.Key, resourceRow.Value);
            }
            // TODO: political updates, building ownership updates
        }
    }
}