using System;
using System.Diagnostics;
using System.Text;

namespace Lab10;

public static class Benchmarks
{
    public const int VariantN = 25;
    public const int TopK = 10;

    public static double MeasureMinMs(Action action)
    {
        action();

        double minMs = double.MaxValue;
        for (int i = 0; i < 3; i++)
        {
            var sw = Stopwatch.StartNew();
            action();
            sw.Stop();

            double ms = sw.Elapsed.TotalMilliseconds;
            if (ms < minMs)
                minMs = ms;
        }
        return minMs;
    }

    public static Interval[] GenerateIntervals(Random rnd, int count)
    {
        var arr = new Interval[count];
        for (int i = 0; i < count; i++)
        {
            int a = rnd.Next(0, 48);
            int b = rnd.Next(a + 1, 49);
            arr[i] = new Interval(a, b);
        }
        return arr;
    }

    public static string RepeatText(string source, int times)
    {
        var sb = new StringBuilder(source.Length * times);
        for (int i = 0; i < times; i++)
        {
            sb.Append(source);
            sb.Append(' ');
        }
        return sb.ToString();
    }
}