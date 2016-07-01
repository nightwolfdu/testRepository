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
        private static readonly string [] DefaultLineSeparator = { " > " };

        private static void Main(string[] args) {
            CreateTestCase();
            //Красивый способ
            //            var testDict= GetTestDictFromFile(); //O(n)- O(n^2) 
            //            var result = BeautifulSortClass.BeautifulSort(testDict);
            //            File.WriteAllLines(OutPath,result.Select(pair => pair.Key + " > " + pair.Value));
            //Быстрый способ
            string line;
            var file = new StreamReader(TestCasePath);
            while ((line = file.ReadLine()) != null) {
                var keyAndValue = line.Split(DefaultLineSeparator, StringSplitOptions.RemoveEmptyEntries);//Можно реализовывать свой split, аккуратно разделяя на char[], но откровенно лень.
                var newCard = new Card() {
                    _key = keyAndValue[0],
                    _value = keyAndValue[1]
                };
                Card nextCard = null;
                if (Card.CardKeyToCardsDict.TryGetValue(newCard._value, out nextCard)) {
                    newCard._next = nextCard;
                    nextCard._previous = newCard;
                    Card.possibleFirstNodeKeys.Remove(nextCard._key);
                }
                Card prevCard = null;
                if (Card.CardValueToCardsDict.TryGetValue(newCard._key, out prevCard)) {
                    prevCard._next = newCard;
                    newCard._previous = prevCard;
                } else {
                    Card.possibleFirstNodeKeys.Add(newCard._key);
                }
                Card.CardKeyToCardsDict.Add(newCard._key, newCard);
                Card.CardValueToCardsDict.Add(newCard._value, newCard);
            }
            file.Close();
            File.WriteAllText(OutPath,Card.ToStringCardCollection());
        }

        private class Card {
            public static HashSet<string> possibleFirstNodeKeys = new HashSet<string>();
            public static Dictionary<string,Card> CardKeyToCardsDict = new Dictionary<string, Card>();
            public static Dictionary<string, Card> CardValueToCardsDict = new Dictionary<string, Card>();
            public string _key;
            public string _value;
            public Card _next;
            public Card _previous;
            public override string ToString() {
                // Для длинных строк использование StringBuilder должно быть оправдано, хотя тут надо посмотреть
                var builder = new StringBuilder(_key.Length + _value.Length + DefaultLineSeparator[0].Length);
                builder.Append(_key);
                builder.Append(DefaultLineSeparator[0]);
                builder.Append(_value);
                return builder.ToString();
            }

            public static string ToStringCardCollection() {
                var current = CardKeyToCardsDict[possibleFirstNodeKeys.First()];
                List<string> resultStrings = new List<string>();
                do {
                    resultStrings.Add(current.ToString());
                    current = current._next;
                } while (current != null);
                return resultStrings.StrJoin(Environment.NewLine);
            }
        }


        private static void CreateTestCase() {
            File.WriteAllLines(TestCasePath, new List<string> {
                "Мельбурн > Кельн",
                "Москва > Париж",
                "Кельн > Москва",
//                "Париж > Питер",
//                "Питер > Лондон"
            });
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

public static class StringExtension
{
    public static string StrJoin(this IEnumerable<string> thisStrings, string delimeter)
    {
        var builder = new StringBuilder();
        var enumerator = thisStrings.GetEnumerator();
        if (!enumerator.MoveNext())
        {
            return "";
        }
        builder.Append(enumerator.Current);
        while (enumerator.MoveNext())
        {
            builder.Append(delimeter);
            builder.Append(enumerator.Current);
        }
        return builder.ToString();
    }
}
