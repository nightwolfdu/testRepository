using System.Collections.Generic;
using NUnit.Framework;

namespace TestSortKeyValue.Tests {
    /// <summary>
    /// Тесты на алгоритм быстрой сортировки карточек.
    /// </summary>
    [TestFixture]
    public class FastCardSortTest {
        //NOTE: тестировать буду как черный ящик, ковырять внутренности алгоритма муторно и лишено особого смысла.
        [Test, TestCaseSource(nameof(TestCases))]
        public void AlgorithmTest(Dictionary<string, int> stringAndExpectedIndex) {
            //Arrange
            FastCardSortClass fastCardSort = new FastCardSortClass(new []{" > "});
            //Act
            var sorted = fastCardSort.FastSort(stringAndExpectedIndex.Keys);
            //Assert
            int num = 1; //не по программистки как-то,но NotePad нумерует строки с 1, и править кейсы уже не хочется.
            foreach (var str in sorted) {
                Assert.AreEqual(stringAndExpectedIndex[str],num);
                ++num;
            }
        }
        /// <summary>
        /// Кейсы для теста
        /// </summary>
        public static List<Dictionary<string, int>> TestCases = new List<Dictionary<string, int>> {
            new Dictionary<string, int> {
                {"Мельбурн > Кельн", 2},
                {"Москва > Париж", 4},
                {"Кельн > Москва", 3},
                {"Питер > Лондон", 6},
                {"Париж > Питер", 5},
                {"Караганда > Мельбурн", 1},
                {"Тула > Венев", 8},
                {"Лондон > Тула", 7}
            },
            new Dictionary<string, int> {
                {"Мельбурн > Кельн", 1},
                {"Москва > Париж", 3},
                {"Кельн > Москва", 2},
                {"Питер > Лондон", 5},
                {"Париж > Питер", 4}
            }
        };
    }
}