using System;
using System.IO;
using System.Text;

namespace Lab10;

internal class Program
{
    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        string prosePath = "prose.txt";
        if (!File.Exists(prosePath))
        {
            Console.WriteLine("Помилка: файл prose.txt не знайдено у поточній директорії.");
            return;
        }

        string fullProse = File.ReadAllText(prosePath, Encoding.UTF8);
        if (fullProse.Length < 50_000)
        {
            Console.WriteLine($"Увага: довжина prose.txt ({fullProse.Length}) менша за 50 000 символів.");
        }

        string prose50k = fullProse.Length >= 50_000 ? fullProse.Substring(0, 50_000) : fullProse;

        RunTask1Traps(prose50k);
        RunTask2TopK(prose50k);
        RunTask3SubstringSearch(prose50k);
        RunTask4Intervals();
        RunTableC(prose50k);
        RunEdgeCases();
    }

    private static void RunTask1Traps(string prose)
    {
        Console.WriteLine("=== Завдання 1: Три пастки ===");

        double plus10k = Benchmarks.MeasureMinMs(() => StringTasks.Trap1ConcatPlus(10_000));
        double sb10k = Benchmarks.MeasureMinMs(() => StringTasks.Trap1StringBuilder(10_000));
        Console.WriteLine($"Пастка 1 (10 000):  += {plus10k:F4} мс | StringBuilder {sb10k:F4} мс");

        double plus100k = Benchmarks.MeasureMinMs(() => StringTasks.Trap1ConcatPlus(100_000));
        double sb100k = Benchmarks.MeasureMinMs(() => StringTasks.Trap1StringBuilder(100_000));
        Console.WriteLine($"Пастка 1 (100 000): += {plus100k:F4} мс | StringBuilder {sb100k:F4} мс");

        string[] words = StringTasks.ExtractWords(prose).ToArray();
        double toLowerMs = Benchmarks.MeasureMinMs(() => StringTasks.Trap2ToLowerCompare(words, 100_000));
        double ordinalMs = Benchmarks.MeasureMinMs(() => StringTasks.Trap2OrdinalIgnoreCase(words, 100_000));
        Console.WriteLine($"Пастка 2 (100 000): ToLower == {toLowerMs:F4} мс | OrdinalIgnoreCase {ordinalMs:F4} мс");

        var (ordSorted, cultSorted) = StringTasks.Trap3CompareSorts();
        Console.WriteLine("Пастка 3 (Ordinal vs uk-UA):");
        for (int i = 0; i < ordSorted.Length; i++)
        {
            string diff = !string.Equals(ordSorted[i], cultSorted[i], StringComparison.Ordinal) ? " <-- РІЗНИЦЯ" : "";
            Console.WriteLine($"  {i + 1,2}. Ordinal: {ordSorted[i],-12} | uk-UA: {cultSorted[i],-12}{diff}");
        }
        Console.WriteLine();
    }

    private static void RunTask2TopK(string prose50k)
    {
        Console.WriteLine("=== Завдання 2: 10 найчастіших слів ===");
        int[] lengths = { 1_000, 10_000, 50_000 };

        foreach (int len in lengths)
        {
            string slice = prose50k.Substring(0, Math.Min(len, prose50k.Length));

            var resNaive = StringTasks.TopKWordsNaive(slice, Benchmarks.TopK);
            var resFast = StringTasks.TopKWordsFast(slice, Benchmarks.TopK);

            if (!StringTasks.AreTopKEqual(resNaive, resFast))
                throw new Exception($"Результати наївного і швидкого алгоритмів не збігаються на len = {len}!");

            double naiveMs = Benchmarks.MeasureMinMs(() => StringTasks.TopKWordsNaive(slice, Benchmarks.TopK));
            double fastMs = Benchmarks.MeasureMinMs(() => StringTasks.TopKWordsFast(slice, Benchmarks.TopK));
            double ratio = fastMs > 0 ? naiveMs / fastMs : 0;

            Console.WriteLine($"Символів: {len,-6} | Наївно: {naiveMs,8:F4} мс | Швидко: {fastMs,8:F4} мс | У скільки разів: {ratio,6:F1}");
        }

        var top10 = StringTasks.TopKWordsFast(prose50k, Benchmarks.TopK);
        Console.WriteLine("Топ-10 слів на 50 000 символах:");
        foreach (var item in top10)
        {
            Console.WriteLine($"  {item.Word}: {item.Count}");
        }
        Console.WriteLine();
    }

    private static void RunTask3SubstringSearch(string prose50k)
    {
        Console.WriteLine("=== Завдання 3: Пошук підрядка ===");

        var words = StringTasks.ExtractWords(prose50k);
        string existingWord = words[words.Count - 5];
        string missingWord = "абракадабранеіснуючеслово";

        string repeatsText = new string('О', 10_000);
        string repeatsPattern = new string('О', 1089) + "П";

        RunSingleSearchTest("Проза 50к (є в кінці)", prose50k, existingWord);
        RunSingleSearchTest("Проза 50к (немає в тексті)", prose50k, missingWord);
        RunSingleSearchTest("Повтори 10к (шаблон 1090)", repeatsText, repeatsPattern);
        Console.WriteLine();
    }

    private static void RunSingleSearchTest(string label, string text, string pattern)
    {
        int idxOwn = StringTasks.NaiveIndexOf(text, pattern, out int comparisons);
        int idxStd = text.IndexOf(pattern, StringComparison.Ordinal);

        if (idxOwn != idxStd)
            throw new Exception($"Розбіжність IndexOf на тесті {label}: own={idxOwn}, std={idxStd}");

        double ownMs = Benchmarks.MeasureMinMs(() => StringTasks.NaiveIndexOf(text, pattern, out _));
        double stdMs = Benchmarks.MeasureMinMs(() => text.IndexOf(pattern, StringComparison.Ordinal));

        Console.WriteLine($"{label,-28} | Порівнянь: {comparisons,10} | Власний: {ownMs,8:F4} мс | string.IndexOf: {stdMs,8:F4} мс");
    }

    private static void RunTask4Intervals()
    {
        Console.WriteLine("=== Завдання 4: Найбільше заявок без перетинів ===");
        var rnd = new Random(Benchmarks.VariantN);

        int optimalCount = 0;
        double worstLagPercent = 0;
        Interval[]? counterExample = null;

        for (int t = 0; t < 100; t++)
        {
            int size = rnd.Next(1, 16);
            Interval[] test = Benchmarks.GenerateIntervals(rnd, size);

            int greedy = IntervalScheduling.SolveGreedy(test);
            int brute = IntervalScheduling.SolveBruteForce(test);

            if (greedy == brute)
            {
                optimalCount++;
            }
            else
            {
                double lag = (double)(brute - greedy) / brute * 100.0;
                if (lag > worstLagPercent)
                    worstLagPercent = lag;
                if (counterExample == null || test.Length < counterExample.Length)
                    counterExample = test;
            }
        }

        Console.WriteLine($"Тестів з оптимумом: {optimalCount} / 100");
        Console.WriteLine($"Найгірше відставання: {worstLagPercent:F2}%");
        Console.WriteLine($"Найменший контрприклад: {(counterExample == null ? "немає (правило оптимальне)" : counterExample.Length.ToString())}");

        var rndBench = new Random(Benchmarks.VariantN);
        Interval[] data15 = Benchmarks.GenerateIntervals(rndBench, 15);
        Interval[] data20 = Benchmarks.GenerateIntervals(rndBench, 20);

        double brute15Ms = Benchmarks.MeasureMinMs(() => IntervalScheduling.SolveBruteForce(data15));
        double brute20Ms = Benchmarks.MeasureMinMs(() => IntervalScheduling.SolveBruteForce(data20));
        Console.WriteLine($"Час перебору: на 15 елементах = {brute15Ms:F4} мс | на 20 елементах = {brute20Ms:F4} мс");

        Interval[] breakEarliestStart = { new(0, 48), new(1, 5), new(6, 10) };
        Console.WriteLine($"Злам «хто почався раніше» [0..48, 1..5, 6..10]: хибне={IntervalScheduling.SolveWrongEarliestStart(breakEarliestStart)}, оптимум={IntervalScheduling.SolveGreedy(breakEarliestStart)}");

        Interval[] breakShortest = { new(0, 10), new(9, 12), new(11, 20) };
        Console.WriteLine($"Злам «хто коротший» [0..10, 9..12, 11..20]: хибне={IntervalScheduling.SolveWrongShortestDuration(breakShortest)}, оптимум={IntervalScheduling.SolveGreedy(breakShortest)}");
        Console.WriteLine();
    }

    private static void RunTableC(string prose50k)
    {
        Console.WriteLine("=== Додатково (Таблиця C, c = 0): Швидке рішення на тексті x1, x10, x100 ===");
        int[] multipliers = { 1, 10, 100 };

        foreach (int mult in multipliers)
        {
            string text = mult == 1 ? prose50k : Benchmarks.RepeatText(prose50k, mult);
            double ms = Benchmarks.MeasureMinMs(() => StringTasks.TopKWordsFast(text, Benchmarks.TopK));
            Console.WriteLine($"Текст x{mult,-3} ({text.Length,9} символів) | Час: {ms,8:F4} мс");
        }
        Console.WriteLine();
    }

    private static void RunEdgeCases()
    {
        Console.WriteLine("=== Перевірка крайових випадків ===");

        var e1 = StringTasks.TopKWordsFast("", Benchmarks.TopK);
        Console.WriteLine($"Порожній рядок (зав. 2): count = {e1.Count}");

        int e2 = StringTasks.NaiveIndexOf("", "abc", out _);
        Console.WriteLine($"Порожній текст (зав. 3): index = {e2}");

        var e3 = StringTasks.TopKWordsFast("Я", Benchmarks.TopK);
        Console.WriteLine($"Один символ (зав. 2): {e3[0].Word}:{e3[0].Count}");

        var e4 = StringTasks.TopKWordsFast("аааааааа", Benchmarks.TopK);
        Console.WriteLine($"Усі однакові (зав. 2): {e4[0].Word}:{e4[0].Count}");

        var e5 = StringTasks.TopKWordsFast("   слово1   слово2  слово1   ", Benchmarks.TopK);
        Console.WriteLine($"Зайві пробіли (зав. 2): {e5[0].Word}:{e5[0].Count}, {e5[1].Word}:{e5[1].Count}");

        int e6 = StringTasks.NaiveIndexOf("короткий", "дуже довгий шаблон", out _);
        Console.WriteLine($"Шаблон довший за текст (зав. 3): index = {e6}");

        int e7 = StringTasks.NaiveIndexOf("текст", "", out _);
        Console.WriteLine($"Порожній шаблон (зав. 3): index = {e7}");

        int e8Zero = IntervalScheduling.SolveGreedy(Array.Empty<Interval>());
        int e8One = IntervalScheduling.SolveGreedy(new[] { new Interval(5, 10) });
        Console.WriteLine($"0 та 1 елемент (зав. 4): 0 -> {e8Zero}, 1 -> {e8One}");
    }
}