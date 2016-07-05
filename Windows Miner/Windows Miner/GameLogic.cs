using Windows_Miner.GameLogic;

namespace Windows_Miner {
    public class EmptyField : Field {
        public override FieldType Type => FieldType.Empty;
    }

    public class NumberField : Field {
        public override FieldType Type => FieldType.Number;
        public int Number { get; set; }
    }

    public class BombField : Field{
        public override FieldType Type => FieldType.Bomb;
    }

    public enum FieldType {
        None = 0 ,
        Empty = 1,
        Bomb =2,
        Number = 3
    }
}
