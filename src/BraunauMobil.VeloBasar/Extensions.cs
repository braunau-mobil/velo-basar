using System;
using System.Collections.Generic;

namespace BraunauMobil.VeloBasar
{
    public static class Extensions
    {
        public static void AddIfNotNull<T>(this IList<T> list, T value) where T : class
        {
            if (value != null)
            {
                list.Add(value);
            }
        }

        public static T TakeRandom<T>(this T[] array, Random rand)
        {
            return array[rand.Next(0, array.Length - 1)];
        }

        public static double NextGaussian(this Random rand, double mean, double stdDev)
        {
            //  yehaw random stackoverflow code: https://stackoverflow.com/questions/218060/random-gaussian-variables
            var u1 = 1.0 - rand.NextDouble();
            var u2 = 1.0 - rand.NextDouble();
            var randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);

            return mean + stdDev * randStdNormal;
        }
    }
}
