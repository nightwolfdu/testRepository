using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestSortKeyValue {
    /// <summary>
    /// Класс быстрой сортировки карточек !Пункт Отправления! => !Пункт назначения!. TODO: задать начальную емкость словарям, если на передают в качестве коллеции - коллекцию с известным Count без перебора
    /// </summary>
    public class FastCardSortClass {
        /// <summary>
        /// Разделитель между !Пункт Отправления!  и !Пункт назначения!
        /// </summary>
        public string[] Separator { get; set; }

        /// <summary>
        /// Ключи возможных первых узлов отсортированного списка
        /// </summary>
        public static HashSet<string> PossibleFirstNodeKeys = new HashSet<string>();
        /// <summary>
        /// Словарь !Пункт Отправления! -> карточка
        /// </summary>
        private readonly Dictionary<string, Card> _cardKeyToCardsDict = new Dictionary<string, Card>();//Для большого числа карточек нужно эмпирически задать стартовую ёмкость "сколько не жалко оперативки"
        /// <summary>
        /// Словарь !Пункт назначения! -> карточка
        /// </summary>
        private readonly Dictionary<string, Card> _cardValueToCardsDict = new Dictionary<string, Card>();//Для большого числа карточек нужно эмпирически задать стартовую ёмкость "сколько не жалко оперативки"

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="separator"></param>
        public FastCardSortClass(string[] separator) {
            Separator = separator;
        }

        /// <summary>
        /// Функция сортировки
        /// </summary>
        public IEnumerable<string> FastSort(IEnumerable<string> inputStrings) {
            foreach (var line in inputStrings) {//O(n)
                var newCard = new Card(line, Separator);
                BindNextCard(newCard);//В теории О(1). 
                BindPreviousCard(newCard);//В теории О(1)
                AddCardToDicts(newCard); //В теории О(1)
            }
            return GetCardCollection();//О(n)
        }
        /// <summary>
        /// Добавление новой карточки в индексы
        /// </summary>
        /// <param name="newCard">новая карточка</param>
        private void AddCardToDicts(Card newCard) {
            _cardKeyToCardsDict.Add(newCard.Key, newCard);
            _cardValueToCardsDict.Add(newCard.Value, newCard);
        }
        /// <summary>
        /// Связываем новую карточку с стоящей перед ней(если такая имеется)
        /// </summary>
        /// <param name="newCard">Новая карточка</param>
        private void BindPreviousCard(Card newCard) {
            Card prevCard;
            if (_cardValueToCardsDict.TryGetValue(newCard.Key, out prevCard)) {
                prevCard.Next = newCard;
            } else {
                PossibleFirstNodeKeys.Add(newCard.Key);
            }
        }
        /// <summary>
        /// Связываем новую карточку с стоящей после нее(если такая имеется)
        /// </summary>
        /// <param name="newCard">Новая карточка</param>
        private void BindNextCard(Card newCard) {
            Card nextCard;
            if (_cardKeyToCardsDict.TryGetValue(newCard.Value, out nextCard)) {
                newCard.Next = nextCard;
                PossibleFirstNodeKeys.Remove(nextCard.Key);
            }
        }
        /// <summary>
        /// Собираем коллекцию карточек в нужном порядке
        /// </summary>
        /// <returns></returns>
        private List<string> GetCardCollection() {
            var current = _cardKeyToCardsDict[PossibleFirstNodeKeys.First()];
            var resultStrings = new List<string>();
            do {
                resultStrings.Add(current.ToString());
                current = current.Next;
            } while (current != null);
            return resultStrings;
        }

        /// <summary>
        /// Карточка
        /// </summary>
        private class Card {
            /// <summary>
            /// Разделитель между !Пункт Отправления!  и !Пункт назначения!
            /// </summary>
            private readonly string _separator;//В принципе, можно сделать статическим и устанавливать один раз.

            /// <summary>
            /// Конструктор
            /// </summary>
            /// <param name="line">Исходная строка</param>
            /// <param name="separator">Требуемый разделитель</param>
            public Card(string line, string [] separator) {
                //Можно реализовывать свой split, аккуратно разделяя на char[], но откровенно лень.
                var keyAndValue = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                _separator = separator.First();
                Key = keyAndValue[0];
                Value = keyAndValue[1];
            }
            /// <summary>
            /// Пункт Отправления
            /// </summary>
            public readonly string Key;
            /// <summary>
            /// Пункт назначения
            /// </summary>
            public readonly string Value;

            /// <summary>
            /// Следующая карточка
            /// </summary>
            public Card Next;
            /// <summary>
            /// Приведение к строке
            /// </summary>
            /// <returns>строка Пункт Отправления + разделитель + Пункт Назначения</returns>
            public override string ToString() {
                // Для длинных строк использование StringBuilder должно быть оправдано, хотя тут надо посмотреть
                var builder = new StringBuilder(Key.Length + Value.Length + _separator.Length);
                builder.Append(Key);
                builder.Append(_separator);
                builder.Append(Value);
                return builder.ToString();
            }
        }
    }
}