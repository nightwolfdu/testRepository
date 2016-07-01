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
        private static readonly string[] DefaultLineSeparator = {" > "};

        private static void Main(string[] args) {
            CreateTestCase();
            //Красивый способ
            //            var testDict= GetTestDictFromFile(); //O(n)- O(n^2) 
            //            var result = BeautifulSortClass.BeautifulSort(testDict);
            //            File.WriteAllLines(OutPath,result.Select(pair => pair.Key + " > " + pair.Value));
            //Быстрый способ
            new FastCardSortClass(TestCasePath, DefaultLineSeparator, OutPath).Do();
        }


        private static void CreateTestCase() {
            File.WriteAllLines(TestCasePath, new List<string> {
                "Мельбурн > Кельн",
                "Москва > Париж",
                "Кельн > Москва",
                "Питер > Лондон",
                "Париж > Питер"
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

public static class StringExtension {
    public static string StrJoin(this IEnumerable<string> thisStrings, string delimeter) {
        var builder = new StringBuilder();
        var enumerator = thisStrings.GetEnumerator();
        if (!enumerator.MoveNext()) {
            return "";
        }
        builder.Append(enumerator.Current);
        while (enumerator.MoveNext()) {
            builder.Append(delimeter);
            builder.Append(enumerator.Current);
        }
        return builder.ToString();
    }
}

public class FastCardSortClass {
    public string TestCasePath { get; set; }
    public string[] DefaultLineSeparator { get; set; }
    public string OutPath { get; set; }

    public FastCardSortClass(string testCasePath, string[] defaultLineSeparator, string outPath) {
        TestCasePath = testCasePath;
        DefaultLineSeparator = defaultLineSeparator;
        OutPath = outPath;
    }

    public IEnumerable<string> ReadInput() {
        return File.ReadAllLines(TestCasePath);//Можно переписать на построчное чтение и коллекцию с итератором. Чтобы IEnumerable читался действительно построчно из файла. Но уже лень.
    }
    public void WriteResult(IEnumerable<string> result){
        File.WriteAllLines(OutPath, result);
    }

    public void Do() {
        WriteResult(FastSort(ReadInput()));
    }

    public IEnumerable<string> FastSort(IEnumerable<string> inputStrings) {
        foreach (var line in inputStrings) {
            var keyAndValue = line.Split(DefaultLineSeparator, StringSplitOptions.RemoveEmptyEntries);
            //Можно реализовывать свой split, аккуратно разделяя на char[], но откровенно лень.
            var newCard = new Card(DefaultLineSeparator[0]) {
                Key = keyAndValue[0],
                Value = keyAndValue[1]
            };
            Card nextCard;
            if (Card.CardKeyToCardsDict.TryGetValue(newCard.Value, out nextCard)) {
                newCard.Next = nextCard;
                Card.PossibleFirstNodeKeys.Remove(nextCard.Key);
            }
            Card prevCard;
            if (Card.CardValueToCardsDict.TryGetValue(newCard.Key, out prevCard)) {
                prevCard.Next = newCard;
            }
            else {
                Card.PossibleFirstNodeKeys.Add(newCard.Key);
            }
            Card.CardKeyToCardsDict.Add(newCard.Key, newCard);
            Card.CardValueToCardsDict.Add(newCard.Value, newCard);
        }
        return Card.ToCardCollection();
    }
}

public class Card {
    private readonly string _defaultLineSeparator;

    public Card(string defaultLineSeparator) {
        _defaultLineSeparator = defaultLineSeparator;
    }

    public static HashSet<string> PossibleFirstNodeKeys = new HashSet<string>();
    public static Dictionary<string, Card> CardKeyToCardsDict = new Dictionary<string, Card>();
    public static Dictionary<string, Card> CardValueToCardsDict = new Dictionary<string, Card>();
    public string Key;
    public string Value;
    public Card Next;

    public override string ToString() {
        // Для длинных строк использование StringBuilder должно быть оправдано, хотя тут надо посмотреть
        var builder = new StringBuilder(Key.Length + Value.Length + _defaultLineSeparator.Length);
        builder.Append(Key);
        builder.Append(_defaultLineSeparator);
        builder.Append(Value);
        return builder.ToString();
    }

    public static string ToStringCardCollection() {
        return ToCardCollection().StrJoin(Environment.NewLine);
    }
    public static List<string> ToCardCollection() {
        var current = CardKeyToCardsDict[PossibleFirstNodeKeys.First()];
        var resultStrings = new List<string>();
        do {
            resultStrings.Add(current.ToString());
            current = current.Next;
        } while (current != null);
        return resultStrings;
    }
}




