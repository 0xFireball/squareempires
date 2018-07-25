using System;
using System.Collections.Generic;
using WireSpire.Entities;
using WireSpire.Types;

namespace WireSpire {
    public class EmpireGenerator {
        public class EmpireTemplates {
            public static void emergent(Empire empire) {
                empire.baseIncome.addResource(Resource.INFLUENCE, 2);
            }
        }

        public Empire createEmpire(Action<Empire> template) {
            var empire = new Empire();
            // apply the template
            template(empire);
            return empire;
        }

        public void placeEmpires(List<Empire> empires, Map map) {
            // TODO: this is just bad please fix
            // place each empire near the corners
            var positions = new[] {
                new Position(1, 1), new Position(map.size.x - 2, 1),
                new Position(map.size.x - 2, map.size.y - 2), new Position(1, map.size.y - 2)
            };
            for (var i = 0; i < empires.Count; i++) {
                var capital = new Station(empires[i], positions[i], Station.Level.Outpost);
                empires[i].capital = capital;
                empires[i].addBuilding(capital);
                // add a scout to each capital
                var scout = new ScoutPawn(empires[i], positions[i]);
                empires[i].addPawn(scout);
            }
        }
    }
}