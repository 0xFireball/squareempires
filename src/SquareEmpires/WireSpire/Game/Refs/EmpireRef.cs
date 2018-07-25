namespace WireSpire.Refs {
    public class EmpireRef {
        public int id;
        public string name;

        public EmpireRef() { }

        public EmpireRef(Empire empire) {
            id = empire.id;
        }
    }
}