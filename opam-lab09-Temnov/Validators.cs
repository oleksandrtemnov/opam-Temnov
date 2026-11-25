using System;
using System.Collections.Generic;

namespace Lab09;

public static class Validators
{
    public static bool IsSorted<T>(T[] a, Func<T, T, int> cmp)
    {
        for (int i = 1; i < a.Length; i++)
        {
            if (cmp(a[i - 1], a[i]) > 0)
                return false;
        }
        return true;
    }

    public static bool IsPermutationOf<T>(T[] original, T[] sorted) where T : notnull
    {
        if (original.Length != sorted.Length)
            return false;

        var counts = new Dictionary<T, int>(original.Length);
        foreach (var item in original)
        {
            counts.TryGetValue(item, out int c);
            counts[item] = c + 1;
        }

        foreach (var item in sorted)
        {
            if (!counts.TryGetValue(item, out int c) || c == 0)
                return false;
            counts[item] = c - 1;
        }

        return true;
    }

    public static (bool IsStable, string CounterExample) CheckStability(Student[] sortedByScore)
    {
        for (int i = 1; i < sortedByScore.Length; i++)
        {
            var prev = sortedByScore[i - 1];
            var curr = sortedByScore[i];

            if (prev.Score == curr.Score && string.Compare(prev.Name, curr.Name, StringComparison.Ordinal) > 0)
            {
                return (false, $"{prev} стоїть перед {curr}");
            }
        }
        return (true, "—");
    }
}