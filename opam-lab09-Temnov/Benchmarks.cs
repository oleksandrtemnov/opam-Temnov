using System;
using System.Diagnostics;

namespace Lab09;

public static class Benchmarks
{
    public const int VariantN = 25;
    public const int VariantK = 19;
    public const int MaxValue = 380;

    public static int[] GenerateRandomArray(int size, int seed = VariantN)
    {
        var rnd = new Random(seed);
        int[] arr = new int[size];
        for (int i = 0; i < size; i++)
            arr[i] = rnd.Next(0, MaxValue + 1);
        return arr;
    }

    public static (int[] Random, int[] Sorted, int[] Reversed, int[] AlmostSorted) GenerateFourInputs(int size)
    {
        int[] random = GenerateRandomArray(size);

        int[] sorted = (int[])random.Clone();
        Array.Sort(sorted);

        int[] reversed = (int[])sorted.Clone();
        Array.Reverse(reversed);

        int[] almost = (int[])sorted.Clone();
        var rnd = new Random(VariantN);
        for (int i = 0; i < VariantK && size > 1; i++)
        {
            int idx1 = rnd.Next(0, size);
            int idx2 = rnd.Next(0, size);
            (almost[idx1], almost[idx2]) = (almost[idx2], almost[idx1]);
        }

        return (random, sorted, reversed, almost);
    }

    public static double MeasureMinMs(Action<int[]> sortAction, int[] template)
    {
        int[] warmup = (int[])template.Clone();
        sortAction(warmup);

        double minMs = double.MaxValue;
        for (int run = 0; run < 3; run++)
        {
            int[] copy = (int[])template.Clone();
            var sw = Stopwatch.StartNew();
            sortAction(copy);
            sw.Stop();

            double ms = sw.Elapsed.TotalMilliseconds;
            if (ms < minMs) minMs = ms;
        }
        return minMs;
    }

    public static int FindOneSecondThreshold()
    {
        int low = 10_000;
        int high = 200_000;
        int result = high;

        while (true)
        {
            var data = GenerateRandomArray(high);
            double ms = MeasureMinMs(arr => Sorts.InsertionSort(arr, (x, y) => x.CompareTo(y), out _, out _), data);
            if (ms >= 1000.0) break;
            low = high;
            high *= 2;
        }

        while (high - low > 1000)
        {
            int mid = low + (high - low) / 2;
            var data = GenerateRandomArray(mid);
            double ms = MeasureMinMs(arr => Sorts.InsertionSort(arr, (x, y) => x.CompareTo(y), out _, out _), data);
            if (ms >= 1000.0)
            {
                result = mid;
                high = mid;
            }
            else
            {
                low = mid;
            }
        }
        return result;
    }
}