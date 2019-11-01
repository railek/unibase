using System;
using System.Collections.Generic;

namespace Railek.Unibase.Extensions
{
    public static class ListExtensions
    {
        public static T Random<T>(this IList<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static T RemoveRandom<T>(this IList<T> list)
        {
            var indexToRemoveAt = UnityEngine.Random.Range(0, list.Count);
            var item = list[indexToRemoveAt];

            list.RemoveAt(indexToRemoveAt);

            return item;
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            var rng = new Random();

            for (var i = list.Count - 1; i > 1; i--)
            {
                var k = rng.Next(i);
                var value = list[k];
                list[k] = list[i];
                list[i] = value;
            }
        }
    }
}
