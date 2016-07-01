using System.Collections.Generic;
using NUnit.Framework;

namespace TestSortKeyValue.Tests {
    /// <summary>
    /// Тесты на функции отдельной карточки.
    /// </summary>
    [TestFixture(Description = "Тесты на функции отдельной карточки.")]
    public class CardTests {

        /// <summary>
        /// Тест на сборку объекта карточки из строки заданного формата
        /// </summary>
        [TestCase("Мельбурн > Киев", new[] {" > "}, "Мельбурн", "Киев", " > ")]
        [TestCase("Киев - Караганда", new[] {" - "}, "Киев", "Караганда", " - ")]
        public void ConstructorTest(string line, string[] separator, string expectedKey, string expectedValue,
            string expectedSeparator) {
            //Arrange
            //Act
            var card = new Card(line, separator);
            //Assert
            Assert.AreEqual(expectedKey, card.Key);
            Assert.AreEqual(expectedValue, card.Value);
            Assert.AreEqual(expectedSeparator, card._separator);
        }

        /// <summary>
        /// Тест на выведение строкового представления карточки
        /// </summary>
        [Test, TestCaseSource(nameof(TestCards))]
        public void TestCardToString(KeyValuePair<Card, string> cardAndExpectedString) {
            //Arrange
            var card = cardAndExpectedString.Key;
            var expectedString = cardAndExpectedString.Value;
            //Act
            var result = card.ToString();
            //Assert
            Assert.AreEqual(expectedString, result);
        }

        public static Dictionary<Card, string> TestCards = new Dictionary<Card, string> {
            {new Card("Мельбурн", "Киев", " > "), "Мельбурн > Киев"},
            {new Card("Киев", "Караганда", " - "), "Киев - Караганда"}
        };
    }
}