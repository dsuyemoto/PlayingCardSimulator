using System;

namespace CardDeck
{
    public class RandomNumberGeneratorCustom
    {
        public static int GetNumber(int size)
        {
            var rng = new Random();

            return rng.Next(1, size);
        }
    }
}
