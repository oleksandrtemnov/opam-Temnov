using System;
using System.Diagnostics;

namespace SearchLab
{
    internal class Program
    {
        private static readonly string[] Alphabet = new string[]
        {
            "А", "Б", "В", "Г", "Ґ", "Д", "Е", "Є", "Ж", "З",
            "И", "І", "Ї", "Й", "К", "Л", "М", "Н", "О", "П",
            "Р", "С", "Т", "У", "Ф", "Х", "Ц", "Ч", "Ш", "Щ",
            "Ь", "Ю", "Я"
        };

        private static string[] GenerateArray(int size, int seed)
        {
            Random rng = new Random(seed);
            string[] arr = new string[size];
            for (int i = 0; i < size; i++)
            {
                string letter = Alphabet[rng.Next(Alphabet.Length)];
                int number = rng.Next(0, 100);
                arr[i] = $"{letter}-{number:D2}";
            }
            return arr;
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            int N = 25;
            Func<string, string, int> cmp = (x, y) => string.Compare(x, y);

            Console.WriteLine("=== ГЕНЕРАЦІЯ ТА СОРТУВАННЯ МАСИВУ (100 000 елементів) ===");
            string[] unsortedData = GenerateArray(100000, N);
            string[] sortedData = (string[])unsortedData.Clone();
            Array.Sort(sortedData, (x, y) => cmp(x, y));

            string X = "О-00";
            string Y = "О-99";
            string absent = "Я-99";
            string firstElem = sortedData[0];

            Console.WriteLine($"X = {X}, Y = {Y}, Відсутній = {absent}, Перший = {firstElem}\n");

            Console.WriteLine("--- ЗАВДАННЯ 1: Лінійний пошук ---");
            Search.LinearSearch(sortedData, X, cmp, out int compX);
            Search.LinearSearch(sortedData, firstElem, cmp, out int compFirst);
            Search.LinearSearch(sortedData, absent, cmp, out int compAbs);

            Console.WriteLine($"1. X ({X}): Порівнянь = {compX}");
            Console.WriteLine($"2. Перший ({firstElem}): Порівнянь = {compFirst}");
            Console.WriteLine($"3. Відсутній ({absent}): Порівнянь = {compAbs}\n");

            Console.WriteLine("--- ЗАВДАННЯ 2: Правильний та зламані бінарні пошуки ---");
            Search.BinarySearch(sortedData, X, cmp, out int bCompX);
            Console.WriteLine($"Правильний Бінарний -> X: {bCompX} порівнянь");

            int b1Res = Broken.Broken1_Condition(sortedData, X, cmp, out _);
            Console.WriteLine($"[Поломка 1 (lo < hi)] Шукаємо {X}: Повернуто {b1Res}");

            Broken.Broken2_InfiniteLoop(sortedData, absent, cmp, out _, out bool hanged);
            Console.WriteLine($"[Поломка 2 (lo = mid)] Зависання на {absent}? -> {hanged}");

            int b3Res = Broken.Broken3_Overflow(sortedData, X, cmp, out int b3Comp);
            Console.WriteLine($"[Поломка 3 ((lo+hi)/2)] Результат: {b3Res}, Порівнянь {b3Comp}");

            int failedCount = 0;
            Random rand = new Random(42);
            for (int i = 0; i < 100; i++)
            {
                string target = sortedData[rand.Next(sortedData.Length)];
                if (Broken.Broken4_UnsortedSearch(unsortedData, target, cmp, out _) == -1)
                    failedCount++;
            }
            Console.WriteLine($"[Поломка 4 (Невідсортований)] Не знайдено з 100: {failedCount}\n");

            Console.WriteLine("--- ЗАВДАННЯ 3: Пошук межі ---");
            int lbIdx = Search.LowerBound(sortedData, X, cmp, out int lbComp);
            int ubIdx = Search.UpperBound(sortedData, X, cmp, out int ubComp);
            Console.WriteLine($"LowerBound для {X}: Індекс {lbIdx}, Порівнянь = {lbComp}");
            Console.WriteLine($"UpperBound для {X}: Індекс {ubIdx}, Порівнянь = {ubComp}");
            Console.WriteLine($"Індекс останнього входження X (b=1): {ubIdx - 1}");
            Console.WriteLine($"Кількість елементів = X: {ubIdx - lbIdx}\n");

            Console.WriteLine("--- ЗАВДАННЯ 4: Рекурсивний бінарний пошук (c = 0) ---");
            Search.RecursiveBinarySearch(sortedData, X, cmp, out int rCompX, out int maxDepthX);
            Console.WriteLine($"Рекурсивний -> X: {rCompX} порівнянь, Макс. глибина = {maxDepthX}\n");

            Console.WriteLine("--- ЗАВДАННЯ 5: Вимірювання продуктивності ---");
            RunTask5Measurements(cmp, N);
        }

        private static void RunTask5Measurements(Func<string, string, int> cmp, int seed)
        {
            int[] sizes = { 1000, 10000, 100000, 1000000 };
            Console.WriteLine("Таблиця 1: Кількість порівнянь");
            foreach (int n in sizes)
            {
                string[] data = GenerateArray(n, seed);
                Array.Sort(data, (x, y) => cmp(x, y));
                long linTotal = 0, binTotal = 0;
                Random rng = new Random(123);
                for (int q = 0; q < 100; q++)
                {
                    string target = data[rng.Next(data.Length)];
                    Search.LinearSearch(data, target, cmp, out int l);
                    Search.BinarySearch(data, target, cmp, out int b);
                    linTotal += l;
                    binTotal += b;
                }
                Console.WriteLine($"n={n,-8} | Лінійний: {linTotal / 100.0,-7} | Бінарний: {binTotal / 100.0,-5}");
            }

            Console.WriteLine("\nТаблиця 2: Точка окупності (100 000 елементів)");
            string[] rawArray = GenerateArray(100000, seed);
            int[] qValues = { 1, 5, 10, 20, 30, 50, 100 };
            foreach (int q in qValues)
            {
                double minLin = double.MaxValue, minSortBin = double.MaxValue;
                for (int r = 0; r < 3; r++)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    for (int i = 0; i < q; i++)
                        Search.LinearSearch(rawArray, rawArray[i % rawArray.Length], cmp, out _);
                    sw.Stop();
                    if (sw.Elapsed.TotalMilliseconds < minLin) minLin = sw.Elapsed.TotalMilliseconds;
                }

                for (int r = 0; r < 3; r++)
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    string[] workArr = (string[])rawArray.Clone();
                    Array.Sort(workArr, (x, y) => cmp(x, y));
                    for (int i = 0; i < q; i++)
                        Search.BinarySearch(workArr, workArr[i % workArr.Length], cmp, out _);
                    sw.Stop();
                    if (sw.Elapsed.TotalMilliseconds < minSortBin) minSortBin = sw.Elapsed.TotalMilliseconds;
                }
                string winner = minLin < minSortBin ? "Лінійний" : "Сорт + Бінарний";
                Console.WriteLine($"q={q,-4} | Лінійний: {minLin,-6:F2} мс | Сорт+Бінарний: {minSortBin,-6:F2} мс | Виграв: {winner}");
            }
        }
    }
}