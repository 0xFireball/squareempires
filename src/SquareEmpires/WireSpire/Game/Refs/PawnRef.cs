﻿using WireSpire.Entities;
using WireSpire.Types;

namespace WireSpire.Refs {
    public class PawnRef {
        public Position pos;
        public int empire;
        public Pawn.Type type;
        public int level = 0;

        public PawnRef() { }

        public PawnRef(Pawn pawn) {
            pos = pawn.pos;
            empire = pawn.empire.id;
            type = pawn.type;
            level = pawn.level;
        }
    }
}