using System;
using System.Collections.Generic;

namespace GenericAlgorithmsLab
{
    public static class Algorithms
    {
        public static List<T> Filter<T>(List<T> src, Func<T, bool> keep)
        {
            List<T> result = new List<T>();
            if (src == null || keep == null) return result;

            foreach (var item in src)
            {
                if (keep(item))
                {
                    result.Add(item);
                }
            }
            return result;
        }

        public static List<TOut> Map<TIn, TOut>(List<TIn> src, Func<TIn, TOut> convert)
        {
            List<TOut> result = new List<TOut>();
            if (src == null || convert == null) return result;

            foreach (var item in src)
            {
                result.Add(convert(item));
            }
            return result;
        }

        public static TAcc Aggregate<T, TAcc>(List<T> src, TAcc start, Func<TAcc, T, TAcc> step)
        {
            TAcc accumulator = start;
            if (src == null || step == null) return accumulator;

            foreach (var item in src)
            {
                accumulator = step(accumulator, item);
            }
            return accumulator;
        }

        public static Dictionary<TKey, List<T>> GroupBy<T, TKey>(List<T> src, Func<T, TKey> keyOf)
        {
            var dict = new Dictionary<TKey, List<T>>();
            if (src == null || keyOf == null) return dict;

            foreach (var item in src)
            {
                TKey key = keyOf(item);
                if (!dict.ContainsKey(key))
                {
                    dict[key] = new List<T>();
                }
                dict[key].Add(item);
            }
            return dict;
        }
    }
}