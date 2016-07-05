namespace Windows_Miner {
    public  class GameLogic {
    }

    public class GameSquare {
    }

    public abstract class Field {
        FieldType type;

        public abstract void OnClickLogic();
    }

    public enum FieldType {
        None = 0 ,
        Empty = 1,
        Bomb =2,
        Number = 3
    }
}
