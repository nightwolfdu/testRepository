using System;
using System.Linq;

namespace Windows_Miner.GameLogic {
    public abstract class Field {
        private const double DefaultBombChance = 0.5;
        public static Random Rand = new Random(DateTime.Now.Millisecond);

        public abstract FieldType Type { get; }


        public virtual void OnClickLogic() {
            Show();
        }

        public static Field GenerateField(Field [] aroundFields) {
            var bombCount = aroundFields.Count(field => field.Type == FieldType.Bomb);
            var emptyCount= aroundFields.Count(field => field.Type == FieldType.Empty);
            if (bombCount == 0 && emptyCount == 0){
                if (Rand.NextDouble() >= DefaultBombChance) {
                    return new BombField();
                }
                return new EmptyField();
            }
            if (emptyCount == 0 && Rand.NextDouble() >= DefaultBombChance) {
                return new BombField();
            }
            return new NumberField { Number= bombCount };
        }

        public void Show() {
        }
    }
}