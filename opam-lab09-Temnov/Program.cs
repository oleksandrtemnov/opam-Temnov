using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab09;

internal class Program
{
    private static readonly Func<int, int, int> IntCmp = (x, y) => x.CompareTo(y);

    private static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        RunTask1TracePreview();
        RunTask4Table();
        RunTask5Benchmarks();
        RunTask5Stability();
        RunEdgeCases();
    }

    private static void RunTask1TracePreview()
    {
        Console.WriteLine("=== Перші 10 елементів масиву (new Random(25), [0; 380]) ===");
        int[] first10 = Benchmarks.GenerateRandomArray(10);
        Console.WriteLine(string.Join(", ", first10));
        Console.WriteLine();
    }

    private static void RunTask4Table()
    {
        Console.WriteLine("=== Завдання 4: Порівняння / Переставляння ===");
        int[] sizes = { 100, 1_000, 10_000 };

        foreach (int n in sizes)
        {
            var (rnd, srt, rev, alm) = Benchmarks.GenerateFourInputs(n);
            var inputs = new (string Name, int[] Data)[]
            {
                ("Випадковий", rnd),
                ("Відсортований", srt),
                ("Навпаки", rev),
                ("Майже", alm)
            };

            Console.WriteLine($"\n--- n = {n} ---");
            foreach (var (name, data) in inputs)
            {
                TestAndPrintOps("Вставками", Sorts.InsertionSort, data, name);
                TestAndPrintOps("Злиттям (BU)", Sorts.BottomUpMergeSort, data, name);
                TestAndPrintOps("Підрахунком", Sorts.CountingSortInt, data, name);
            }
        }
        Console.WriteLine();
    }

    private static void TestAndPrintOps(string algName, SortDelegate<int> sort, int[] original, string inputType)
    {
        int[] copy = (int[])original.Clone();
        sort(copy, IntCmp, out int cmp, out int moves);

        if (!Validators.IsSorted(copy, IntCmp) || !Validators.IsPermutationOf(original, copy))
            throw new Exception($"Помилка коректності в {algName} на вході {inputType}!");

        Console.WriteLine($"{algName,-14} | {inputType,-14} | {cmp,10} / {moves,10}");
    }

    private static void RunTask5Benchmarks()
    {
        Console.WriteLine("=== Завдання 5: Час виконання (мс, мін. з 3 запусків у Release) ===");
        int[] sizes = { 1_000, 10_000, 100_000, 1_000_000 };

        foreach (int n in sizes)
        {
            int[] data = Benchmarks.GenerateRandomArray(n);

            string insTime = n <= 100_000
                ? $"{Benchmarks.MeasureMinMs(a => Sorts.InsertionSort(a, IntCmp, out _, out _), data):F4}"
                : "— (понад 100 с)";

            double mergeMs = Benchmarks.MeasureMinMs(a => Sorts.BottomUpMergeSort(a, IntCmp, out _, out _), data);
            double countMs = Benchmarks.MeasureMinMs(a => Sorts.CountingSortInt(a, IntCmp, out _, out _), data);
            double stdMs = Benchmarks.MeasureMinMs(a => Array.Sort(a), data);

            Console.WriteLine($"n = {n,-9} | Вставки: {insTime,-14} | Злиття: {mergeMs,8:F4} | Підрахунок: {countMs,8:F4} | Array.Sort: {stdMs,8:F4}");
        }

        Console.WriteLine("\nПошук межі прийнятності (1 секунда) для сортування вставками...");
        int threshold = Benchmarks.FindOneSecondThreshold();
        Console.WriteLine($"Межа прийнятності (n, де час вперше >= 1000 мс): ~{threshold}\n");
    }

    private static void RunTask5Stability()
    {
        Console.WriteLine("=== Завдання 5: Перевірка стабільності (Student[]) ===");
        Student[] baseStudents =
        {
            new("Бондаренко", "КН-21", 85),
            new("Василенко",  "КН-22", 90),
            new("Григоренко", "КН-21", 75),
            new("Дмитренко",  "КН-23", 85),
            new("Іваненко",   "КН-21", 90),
            new("Коваленко",  "КН-22", 75),
            new("Лисенко",    "КН-23", 85),
            new("Мельник",    "КН-21", 90),
            new("Петренко",   "КН-22", 75),
            new("Шевченко",   "КН-23", 85)
        };

        Func<Student, Student, int> byScore = (s1, s2) => s1.Score.CompareTo(s2.Score);

        var s1 = (Student[])baseStudents.Clone();
        Sorts.InsertionSort(s1, byScore, out _, out _);
        Console.WriteLine($"Вставками:   {Validators.CheckStability(s1)}");

        var s2 = (Student[])baseStudents.Clone();
        Sorts.BottomUpMergeSort(s2, byScore, out _, out _);
        Console.WriteLine($"Злиттям:     {Validators.CheckStability(s2)}");

        var s3 = (Student[])baseStudents.Clone();
        Array.Sort(s3, (a, b) => a.Score.CompareTo(b.Score));
        Console.WriteLine($"Array.Sort:  {Validators.CheckStability(s3)}");

        var s4 = baseStudents.OrderBy(s => s.Score).ToArray();
        Console.WriteLine($"OrderBy:     {Validators.CheckStability(s4)}\n");
    }

    private static void RunEdgeCases()
    {
        Console.WriteLine("=== Крайові випадки ===");
        var cases = new (string Name, int[] Data)[]
        {
            ("Порожній масив", Array.Empty<int>()),
            ("Один елемент", new[] { 42 }),
            ("Два елементи в порядку", new[] { 10, 20 }),
            ("Два елементи навпаки", new[] { 20, 10 }),
            ("Усі елементи однакові", Enumerable.Repeat(77, 20).ToArray()),
            ("Два значення по пів масиву", Enumerable.Repeat(100, 10).Concat(Enumerable.Repeat(5, 10)).ToArray())
        };

        foreach (var (name, data) in cases)
        {
            bool ok1 = CheckAlg(Sorts.InsertionSort, data);
            bool ok2 = CheckAlg(Sorts.BottomUpMergeSort, data);
            bool ok3 = CheckAlg(Sorts.CountingSortInt, data);
            Console.WriteLine($"{name,-28} | Вставки: {ok1} | Злиття: {ok2} | Підрахунок: {ok3}");
        }
    }

    private static bool CheckAlg(SortDelegate<int> sort, int[] original)
    {
        int[] copy = (int[])original.Clone();
        sort(copy, IntCmp, out _, out _);
        return Validators.IsSorted(copy, IntCmp) && Validators.IsPermutationOf(original, copy);
    }

    private delegate void SortDelegate<T>(T[] a, Func<T, T, int> cmp, out int comparisons, out int moves);
}