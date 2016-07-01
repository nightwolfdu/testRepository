using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

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
        private const string OutPathTwo = "./testCaseOut2.txt";
        private static readonly string[] DefaultLineSeparator = {" > "};

        private static void Main(string[] args) {
            CreateTestCase();
            //TODO: для честного теста нужно n прогонов.
            var stopwatch = Stopwatch.StartNew();
            Do();
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            stopwatch.Reset();
            stopwatch.Start();
            File.WriteAllText(OutPathTwo, BeautifulSortClass.BeautifulSort(GetTestDictFromFile()));
            Console.WriteLine(stopwatch.ElapsedMilliseconds);
            Console.ReadLine();
        }

        private static void CreateTestCase() {
            File.WriteAllLines(TestCasePath, new List<string> {
                "Мельбурн > Кельн",
                "Москва > Париж",
                "Кельн > Москва",
//                "Питер > Лондон",
//                "Париж > Питер",
//                "Караганда > Мельбурн",
//                "Тула > Венев",
//                "Лондон > Тула"
            });
        }

        /// <summary>
        /// Прочитать вход
        /// </summary>
        public static IEnumerable<string> ReadInput() {
            //Можно переписать на построчное чтение и коллекцию с итератором. Чтобы IEnumerable читался действительно построчно из файла. Но уже лень.
            return File.ReadAllLines(TestCasePath);
        }

        /// <summary>
        /// Записать результат
        /// </summary>
        /// <param name="result"></param>
        public static void WriteResult(IEnumerable<string> result) {
            File.WriteAllLines(OutPath, result);
        }

        /// <summary>
        /// Выполнить быстрый алгоритм сортировки карточек
        /// </summary>
        public static void Do() {
            WriteResult(new FastCardSortClass(DefaultLineSeparator).FastSort(ReadInput()));
        }

        /// <summary>
        ///     Получение тестового словаря из файла. NOTE: Сложность в отсутствие коллизий - O(n). n добавлений, сложность которых
        ///     O(1) https://msdn.microsoft.com/ru-ru/library/k7z0zy8k(v=vs.110).aspx.
        ///     На самом деле, сложность добавления в Dict.. больше чем O(1), т.к. а)в некоторых условиях его придется расширять;
        ///     б) в случае коллизии ключи будут добавляться в список. Но для строк хэш-функция достаточно хороша.
        /// </summary>
        private static Dictionary<string, string> GetTestDictFromFile() {
            var lines = File.ReadAllLines(TestCasePath);
            // O(n) лучшем случае, ближе к O(n^2) если придется расширять словарь. Быстрее можно сделать через new Dictionary(count) и перелив в новый словарь.
            return
                lines.Select(s => s.Split(DefaultLineSeparator, StringSplitOptions.RemoveEmptyEntries))
                    .ToDictionary(strings => strings[0], strings => strings[1]);
        }
    }
}