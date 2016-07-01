using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestSortKeyValue {
    /// <summary>
    /// Класс красивой сортировки
    /// </summary>
    public class BeautifulSortClass {
        /// <summary>
        /// Красивый способ - создание обратного индекса и поиск элемента которого нет в обратном индексе как первого. 
        /// Сбор по цепочке.
        ///  NOTE: итоговая сложность O(n) теоретическая, O(n^2) с понижающими коэффициентами - практическая. Можно ускорить, если собирать Dictionary не методом расширения, а руками, задав начальную емкость
        /// </summary>
        public static string BeautifulSort(Dictionary<string, string> testDict) {
            var reverseDict = testDict.ToDictionary(pair => pair.Value, pair => pair.Key); //O(n)- O(n^2) 
            // Находим стартовый элемент для сбора цепочки. Лучший случай O(1) - первый же не найдем. Средний - O(n/2), худший - O(n). В теории. На практике чуть хуже, т.к. операция поиска на самом деле не совсем О(1)
            var first = testDict.First(pair => !reverseDict.ContainsKey(pair.Key));
            var builder = new StringBuilder();
            var current = first;
            builder.Append(first.Key);
            builder.Append(" > ");
            builder.Append(first.Value);
            builder.Append(Environment.NewLine);
            for (var i = 1; i < testDict.Count; i++) {
                // Еще O(n) в теории. 
                current = new KeyValuePair<string, string>(current.Value, testDict[current.Value]);
                builder.Append(current.Key);
                builder.Append(" > ");
                builder.Append(current.Value);
                builder.Append(Environment.NewLine);
            }
            return builder.ToString();
        }
    }
}