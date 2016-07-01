using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

//        Пожалуйста, разместите ссылки на код(код должен открываться в браузере, без архивов) на GitHub.
//1. Вы собираетесь совершить долгое путешествие через множество населенных пунктов.Чтобы не запутаться, вы сделали карточки вашего путешествия.Каждая карточка содержит в себе пункт отправления и пункт назначения. 
//Гарантируется, что если упорядочить эти карточки так, чтобы для каждой карточки в упорядоченном списке пункт назначения на ней совпадал с пунктом отправления в следующей карточке в списке, получится один список карточек без циклов и пропусков. 
//Например, у нас есть карточки
//Мельбурн > Кельн
//Москва > Париж
//Кельн > Москва
//Если упорядочить их в соответствии с требованиями выше, то получится следующий список: 
//Мельбурн > Кельн, Кельн > Москва, Москва > Париж
//Требуется: 
//Написать функцию, которая принимает набор неупорядоченных карточек и возвращает набор упорядоченных карточек в соответствии с требованиями выше, то есть в возвращаемом из функции списке карточек для каждой карточки пункт назначения на ней должен совпадать с пунктом отправления на следующей карточке.
//Дать оценку сложности получившегося алгоритма сортировки
//Написать тесты
//Оценивается правильность работы, производительность и читабельность кода


namespace TestSortKeyValue {
    internal class Program {
        private const string TestCasePath = "./testCase.txt";
        private const string OutPath = "./testCaseOut.txt";
        private static void Main(string[] args) {
            CreateTestCase();
            var testDict= GetTestDictFromFile(); //O(n)- O(n^2) 
            var result = BeautifulSort(testDict);
            File.WriteAllLines(OutPath,result.Select(pair => pair.Key + " > " + pair.Value));
        }
        /// <summary>
        /// Красивый способ - создание обратного индекса и поиск элемента которого нет в обратном индексе как первого. Сбор по цепочке. NOTE: итоговая сложность O(n) теоретическая, O(n^2) с понижающими коэффициентами - практическая
        /// </summary>
        private static KeyValuePair<string, string>[] BeautifulSort(Dictionary<string, string> testDict) {
            
            var reverseDict = testDict.ToDictionary(pair => pair.Value, pair => pair.Key); //O(n)- O(n^2) 
            // Находим стартовый элемент для сбора цепочки. Лучший случай O(1) - первый же не найдем. Средний - O(n/2), худший - O(n). В теории. На практике чуть хуже, т.к. операция поиска на самом деле не совсем О(1)
            var first = testDict.First(pair => !reverseDict.ContainsKey(pair.Key));

            var result = new KeyValuePair<string, string>[testDict.Count];
            var current = first;
            result[0] = first;
            for (int i = 1; i < result.Length; i++) {
            // Еще O(n) в теории. 
                current = new KeyValuePair<string, string>(current.Value, testDict[current.Value]);
                result[i] = current;
            }
            return result;
        }

        /// <summary>
        /// Получение тестового словаря из файла. NOTE: Сложность в отсутствие коллизий - O(n). n добавлений, сложность которых O(1) https://msdn.microsoft.com/ru-ru/library/k7z0zy8k(v=vs.110).aspx. 
        /// На самом деле, сложность добавления в Dict.. больше чем O(1), т.к. а)в некоторых условиях его придется расширять; б) в случае коллизии ключи будут добавляться в список. Но для строк хэш-функция достаточно хороша.
        /// </summary>
        private static Dictionary<string, string> GetTestDictFromFile() {
            var lines = File.ReadAllLines(TestCasePath);
            return 
                lines.Select(s => s.Split(new[] {" > "}, StringSplitOptions.RemoveEmptyEntries))
                    .ToDictionary(strings => strings[0], strings => strings[1]); // O(n) лучшем случае, ближе к O(n^2) если придется расширять словарь. Быстрее можно сделать через new Dictionary(count) и перелив в новый словарь.
        }

        private static void CreateTestCase() {
            File.WriteAllLines(TestCasePath, new List<string>() {
                "Париж > Питер",
                "Мельбурн > Кельн",
                "Москва > Париж",
                "Кельн > Москва",
                "Питер > Лондон ",
            });
        }
    }
}