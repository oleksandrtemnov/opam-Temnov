using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lab04ListAlgorithms
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            int N = 25;
            int K = 19;
            int a = 1;
            int b = 1;
            int c = 0;

            Console.WriteLine($"=== Лабораторна робота 4: Алгоритми над списками ===");
            Console.WriteLine($"N = {N}, K = {K}, a = {a}, b = {b}, c = {c}\n");

            // --- Генерація основного списку (20 чисел [-9; 9], Random(25)) ---
            List<int> mainList = GenerateRandomList(20, -9, 9, N);
            Console.Write("Основний список (20 елементів): ");
            PrintList(mainList);
            Console.WriteLine();

            // --- Завдання 1: Побудова списку (кратні 5) ---
            Console.WriteLine("--- Завдання 1: Побудова (елементи кратні 5) ---");
            List<int> multiplesOf5 = BuildMultiplesOf5(mainList);
            Console.Write("Результат: ");
            PrintList(multiplesOf5);
            Console.WriteLine($"Кількість елементів (Count): {multiplesOf5.Count}");
            Console.WriteLine();

            // --- Завдання 2: Вилучення елементів (три варіанти) ---
            Console.WriteLine("--- Завдання 2: Вилучення кратних 5 (нуль теж) ---");
            
            // Варіант 1: Хибний (з початку з RemoveAt)
            List<int> listForWrong = new List<int>(mainList);
            RemoveWrongForward(listForWrong);
            Console.Write("1. Хибний варіант (з початку): ");
            PrintList(listForWrong);

            // Варіант 2: З кінця
            List<int> listForBackward = new List<int>(mainList);
            RemoveCorrectBackward(listForBackward);
            Console.Write("2. З кінця (коректний):       ");
            PrintList(listForBackward);

            // Варіант 3: Новий список
            List<int> listNewCorrect = RemoveCorrectNewList(mainList);
            Console.Write("3. Новий список (коректний):   ");
            PrintList(listNewCorrect);

            bool isSame = CompareLists(listForBackward, listNewCorrect);
            Console.WriteLine($"Перевірка (Варіант 2 == Варіант 3): {isSame}");
            Console.WriteLine();

            // --- Завдання 3: Злиття двох упорядкованих списків ---
            Console.WriteLine("--- Завдання 3: Злиття впорядкованих списків (14 і 9) ---");
            List<int> list1 = GenerateSortedList(14, N);
            List<int> list2 = GenerateSortedList(9, N + 100);

            Console.Write("Список 1 (14 ел.): ");
            PrintList(list1);
            Console.Write("Список 2 (9 ел.):  ");
            PrintList(list2);

            List<int> mergedList = MergeSortedLists(list1, list2);
            Console.Write("Злитий список:     ");
            PrintList(mergedList);

            bool isSorted = CheckIsSorted(mergedList);
            Console.WriteLine($"Перевірка впорядкованості: {isSorted}");
            Console.WriteLine();

            // --- Завдання 4: Розділення на парні та непарні ---
            Console.WriteLine("--- Завдання 4: Розділення на парні та непарні ---");
            SplitEvenOdd(mainList, out List<int> evens, out List<int> odds);

            Console.Write("Парні елементи:   ");
            PrintList(evens);
            Console.Write("Непарні елементи: ");
            PrintList(odds);

            bool countCheck = (evens.Count + odds.Count) == mainList.Count;
            Console.WriteLine($"Перевірка суми Count ({evens.Count} + {odds.Count} == {mainList.Count}): {countCheck}");
            Console.WriteLine();

            // --- Завдання 5: Замір часу (Stopwatch) ---
            Console.WriteLine("--- Завдання 5: Замір часу операцій ---");
            RunBenchmarks();
        }

        // Допоміжний метод виводу списку
        static void PrintList(List<int> list)
        {
            Console.WriteLine($"[{string.Join(", ", list)}]");
        }

        // Допоміжний метод порівняння двох списків
        static bool CompareLists(List<int> a, List<int> b)
        {
            if (a.Count != b.Count) return false;
            for (int i = 0; i < a.Count; i++)
            {
                if (a[i] != b[i]) return false;
            }
            return true;
        }

        // Генератор випадкового списку
        static List<int> GenerateRandomList(int count, int min, int max, int seed)
        {
            Random rnd = new Random(seed);
            List<int> list = new List<int>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(rnd.Next(min, max + 1));
            }
            return list;
        }

        // --- Завдання 1: Побудова (кратні 5) ---
        public static List<int> BuildMultiplesOf5(List<int> source)
        {
            List<int> result = new List<int>(); // Місткість заздалегідь невідома
            for (int i = 0; i < source.Count; i++)
            {
                if (source[i] % 5 == 0)
                {
                    result.Add(source[i]);
                }
            }
            return result;
        }

        // --- Завдання 2 (Варіант 1): Хибний обхід з початку ---
        public static void RemoveWrongForward(List<int> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] % 5 == 0)
                {
                    list.RemoveAt(i);
                }
            }
        }

        // --- Завдання 2 (Варіант 2): Коректний обхід з кінця ---
        public static void RemoveCorrectBackward(List<int> list)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (list[i] % 5 == 0)
                {
                    list.RemoveAt(i);
                }
            }
        }

        // --- Завдання 2 (Варіант 3): Формування нового списку ---
        public static List<int> RemoveCorrectNewList(List<int> source)
        {
            List<int> result = new List<int>();
            for (int i = 0; i < source.Count; i++)
            {
                if (source[i] % 5 != 0)
                {
                    result.Add(source[i]);
                }
            }
            return result;
        }

        // --- Завдання 3: Генерація впорядкованого списку ---
        static List<int> GenerateSortedList(int count, int seed)
        {
            Random rnd = new Random(seed);
            List<int> list = new List<int>(count);
            int current = rnd.Next(1, 10);
            list.Add(current);

            for (int i = 1; i < count; i++)
            {
                current += rnd.Next(1, 10);
                list.Add(current);
            }
            return list;
        }

        // --- Завдання 3: Злиття двох списків за один прохід ---
        public static List<int> MergeSortedLists(List<int> list1, List<int> list2)
        {
            List<int> merged = new List<int>(list1.Count + list2.Count);
            int i = 0, j = 0;

            while (i < list1.Count && j < list2.Count)
            {
                if (list1[i] <= list2[j])
                {
                    merged.Add(list1[i]);
                    i++;
                }
                else
                {
                    merged.Add(list2[j]);
                    j++;
                }
            }

            // Дописування залишків
            while (i < list1.Count)
            {
                merged.Add(list1[i]);
                i++;
            }

            while (j < list2.Count)
            {
                merged.Add(list2[j]);
                j++;
            }

            return merged;
        }

        // --- Завдання 3: Перевірка впорядкованості ---
        public static bool CheckIsSorted(List<int> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i] > list[i + 1])
                {
                    return false;
                }
            }
            return true;
        }

        // --- Завдання 4: Розділення на парні та непарні ---
        public static void SplitEvenOdd(List<int> source, out List<int> evens, out List<int> odds)
        {
            evens = new List<int>();
            odds = new List<int>();

            for (int i = 0; i < source.Count; i++)
            {
                if (source[i] % 2 == 0)
                {
                    evens.Add(source[i]);
                }
                else
                {
                    odds.Add(source[i]);
                }
            }
        }

        // --- Завдання 5: Заміри часу ---
        static void RunBenchmarks()
        {
            // Прогрівальний прогін JIT
            List<int> warmup = new List<int>();
            for (int i = 0; i < 1000; i++) warmup.Add(i);

            int N_ops = 100000;
            Stopwatch sw = new Stopwatch();

            // 1. Add(x) без заданої місткості
            sw.Restart();
            List<int> list1 = new List<int>();
            for (int i = 0; i < N_ops; i++) list1.Add(i);
            sw.Stop();
            long t1 = sw.ElapsedMilliseconds;

            // 2. Add(x) із заданою місткістю 100 000
            sw.Restart();
            List<int> list2 = new List<int>(N_ops);
            for (int i = 0; i < N_ops; i++) list2.Add(i);
            sw.Stop();
            long t2 = sw.ElapsedMilliseconds;

            // 3. Insert(0, x)
            sw.Restart();
            List<int> list3 = new List<int>();
            for (int i = 0; i < N_ops; i++) list3.Insert(0, i);
            sw.Stop();
            long t3 = sw.ElapsedMilliseconds;

            // 4. Contains 1000 разів у списку зі 100 000 елементів
            sw.Restart();
            for (int i = 0; i < 1000; i++)
            {
                bool found = list1.Contains(-999999); // Значення, якого немає
            }
            sw.Stop();
            long t4 = sw.ElapsedMilliseconds;

            Console.WriteLine($"1. 100 000 x Add(x) [без capacity]: {t1} мс");
            Console.WriteLine($"2. 100 000 x Add(x) [capacity=100000]: {t2} мс");
            Console.WriteLine($"3. 100 000 x Insert(0, x): {t3} мс");
            Console.WriteLine($"4. 1 000 x Contains(немає) у 100 000: {t4} мс");
        }
    }
}