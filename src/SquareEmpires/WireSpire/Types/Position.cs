using System;

namespace WireSpire.Types {
    public struct Position {
        public readonly int x;
        public readonly int y;

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

        public override bool Equals(object obj) {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return equalTo((Position) obj);
        }

        public override int GetHashCode() {
            unchecked // Overflow is fine, just wrap
            {
                var hash = 17;
                hash = hash * 23 + x.GetHashCode();
                hash = hash * 23 + y.GetHashCode();
                return hash;
            }
        }

        public bool equalTo(Position p) => (x == p.x) && (y == p.y);

        public override string ToString() => $"({x}, {y})";
    }
}