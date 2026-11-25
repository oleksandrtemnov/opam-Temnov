using System;

namespace Lab10;

public readonly record struct Interval(int Start, int End);

public static class IntervalScheduling
{
    public static int SolveGreedy(Interval[] intervals)
    {
        if (intervals.Length == 0)
            return 0;

        var copy = (Interval[])intervals.Clone();
        Array.Sort(copy, (a, b) =>
        {
            int cmp = a.End.CompareTo(b.End);
            return cmp != 0 ? cmp : a.Start.CompareTo(b.Start);
        });

        int count = 0;
        int lastEnd = int.MinValue;

        for (int i = 0; i < copy.Length; i++)
        {
            if (copy[i].Start >= lastEnd)
            {
                count++;
                lastEnd = copy[i].End;
            }
        }

        return count;
    }

    public static int SolveBruteForce(Interval[] intervals)
    {
        if (intervals.Length == 0)
            return 0;

        var sorted = (Interval[])intervals.Clone();
        Array.Sort(sorted, (a, b) =>
        {
            int cmp = a.Start.CompareTo(b.Start);
            return cmp != 0 ? cmp : a.End.CompareTo(b.End);
        });

        return SearchRecursive(sorted, 0, int.MinValue);
    }

    private static int SearchRecursive(Interval[] sorted, int index, int lastEnd)
    {
        if (index == sorted.Length)
            return 0;

        int skip = SearchRecursive(sorted, index + 1, lastEnd);

        int take = 0;
        if (sorted[index].Start >= lastEnd)
        {
            take = 1 + SearchRecursive(sorted, index + 1, sorted[index].End);
        }

        return take > skip ? take : skip;
    }

    public static int SolveWrongEarliestStart(Interval[] intervals)
    {
        if (intervals.Length == 0)
            return 0;

        var copy = (Interval[])intervals.Clone();
        Array.Sort(copy, (a, b) =>
        {
            int cmp = a.Start.CompareTo(b.Start);
            return cmp != 0 ? cmp : a.End.CompareTo(b.End);
        });

        int count = 0;
        int lastEnd = int.MinValue;

        for (int i = 0; i < copy.Length; i++)
        {
            if (copy[i].Start >= lastEnd)
            {
                count++;
                lastEnd = copy[i].End;
            }
        }

        return count;
    }

    public static int SolveWrongShortestDuration(Interval[] intervals)
    {
        if (intervals.Length == 0)
            return 0;

        var copy = (Interval[])intervals.Clone();
        Array.Sort(copy, (a, b) =>
        {
            int lenA = a.End - a.Start;
            int lenB = b.End - b.Start;
            int cmp = lenA.CompareTo(lenB);
            return cmp != 0 ? cmp : a.Start.CompareTo(b.Start);
        });

        var chosen = new Interval[copy.Length];
        int count = 0;

        for (int i = 0; i < copy.Length; i++)
        {
            bool overlaps = false;
            for (int j = 0; j < count; j++)
            {
                if (copy[i].Start < chosen[j].End && copy[i].End > chosen[j].Start)
                {
                    overlaps = true;
                    break;
                }
            }

            if (!overlaps)
            {
                chosen[count++] = copy[i];
            }
        }

        return count;
    }
}