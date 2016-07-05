using System;
using System.Collections.Generic;
using System.Linq;

namespace Windows_Miner.GameLogic {
    public class GameSquare {
        private readonly Field[][] Fields;
        private static readonly int[] OffsetsValue = { -1, 0, 1 };
        private static readonly List<Offset> AroundOffsets = GenerateOffsets();

        private static List<Offset> GenerateOffsets() {
            return OffsetsValue.SelectMany(
                widthOffset =>
                    OffsetsValue.Select(heightOffset => new Offset(widthOffset, heightOffset))
                        .Where(tuple => tuple.WidthOffset != 0 || tuple.HeightOffset != 0)).ToList();
        } 

        public GameSquare(int width = 9, int height = 9) {
            Fields = new Field[width][];
            for (int widthIndex = 0; widthIndex < width; widthIndex++) {
                Fields[widthIndex] = new Field[height];
            }
            for (int widthIndex = 0; widthIndex < width; widthIndex++) {
                for (int heightIndex = 0; heightIndex < height; heightIndex++) {
                    Fields[widthIndex][heightIndex] =
                        Field.GenerateField(GetAroundFields(Fields, widthIndex, heightIndex));
                }
            }
        }

        private Field[] GetAroundFields(Field[][] allFields, int widthIndex, int heightIndex) {
            return AroundOffsets.Select(tuple => {
                var widthIndexWithOffset = widthIndex + tuple.WidthOffset;
                var heightIndexWithOffset = heightIndex + tuple.HeightOffset;
                if(widthIndexWithOffset >= 0 && widthIndexWithOffset < allFields.Length &&
                   heightIndexWithOffset >= 0 && heightIndexWithOffset < allFields[0].Length) {
                    return allFields[widthIndexWithOffset][heightIndexWithOffset];
                }
                return null;
            }).Where(field => field != null).ToArray();
        }

        public override string ToString() {
            return Fields.Select(fields => fields.Select(field => field.Type.ToString()).StrJoin("\t")).StrJoin(Environment.NewLine);
        }
    }
}