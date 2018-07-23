using System;

namespace WireSpire.Types {
    public struct Position {
        public int x;
        public int y;

        public Position(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public static Position operator +(Position p1, Position p2) {
            return new Position(p1.x + p2.x, p1.y + p2.y);
        }

        public static Position operator -(Position p1, Position p2) {
            return new Position(p1.x - p2.x, p1.y - p2.y);
        }

        public static int tileDistance(Position p1, Position p2) {
            return Math.Abs(p1.x - p2.x) + Math.Abs(p1.y - p2.y);
        }
    }
}