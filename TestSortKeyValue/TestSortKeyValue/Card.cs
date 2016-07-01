using System;
using System.Linq;
using System.Text;

namespace TestSortKeyValue {
    /// <summary>
    /// Карточка
    /// </summary>
    public class Card {
        /// <summary>
        /// Разделитель между !Пункт Отправления!  и !Пункт назначения!
        /// </summary>
        public readonly string _separator; //В принципе, можно сделать статическим и устанавливать один раз.

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="line">Исходная строка</param>
        /// <param name="separator">Требуемый разделитель</param>
        public Card(string line, string[] separator) {
            //Можно реализовывать свой split, аккуратно разделяя на char[], но откровенно лень.
            var keyAndValue = line.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            _separator = separator.First();
            Key = keyAndValue[0];
            Value = keyAndValue[1];
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public Card(string key, string value, string separator) {
            Key = key;
            Value = value;
            _separator = separator;
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