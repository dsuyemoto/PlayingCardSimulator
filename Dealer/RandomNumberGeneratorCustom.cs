using System;

namespace Dealer
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
