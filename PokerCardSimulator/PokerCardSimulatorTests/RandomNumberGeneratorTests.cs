using CardDeck;
using NUnit.Framework;
using PokerCardSimulator;
using System.Security.Cryptography;

namespace PokerCardSimulatorTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Rng_LessThanEqual52_IsTrueTest()
        {
            var number = RandomNumberGeneratorCustom.GetNumber(52);

            Assert.GreaterOrEqual(52, number);
        }

        public void Rng_GreaterThanEqual1_IsTrue()
        {
            var number = RandomNumberGeneratorCustom.GetNumber(52);

            Assert.GreaterOrEqual(number, 1);
        }
    }
}