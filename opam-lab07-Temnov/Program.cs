using System;
using System.Collections.Generic;

namespace GenericAlgorithmsLab
{
    class Program
    {
        static void Main(string[] args)
        {
            int N = 7;
            int K = 19;
            int a = N % 3;
            int b = N % 4;
            int c = N % 5;

            Console.WriteLine($"--- Параметри варіанта: N = {N}, K = {K}, a = {a}, b = {b}, c = {c} ---\n");

            List<int> numbers = GenerateNumbers(N, K);
            List<string> words = GetWords();
            List<Student> students = GetStudents();

            Console.WriteLine("=== Завдання 1: Filter ===");
            
            var filteredNumbers = Algorithms.Filter(numbers, x => x % 3 == 0);
            Console.WriteLine($"Числа кратні 3 (кількість: {filteredNumbers.Count}): {string.Join(", ", filteredNumbers)}");

            char[] vowels = { 'а', 'е', 'є', 'и', 'і', 'ї', 'о', 'у', 'ю', 'я', 'А', 'Е', 'Є', 'И', 'І', 'Ї', 'О', 'У', 'Ю', 'Я' };
            var filteredWords = Algorithms.Filter(words, w => !string.IsNullOrEmpty(w) && Array.IndexOf(vowels, w[0]) >= 0);
            Console.WriteLine($"Рядки, що починаються з голосної (кількість: {filteredWords.Count}): {string.Join(", ", filteredWords)}");

            var filteredStudents = Algorithms.Filter(students, s => s.Group == "КН-3/1");
            Console.WriteLine($"Студенти з групи КН-3/1 (кількість: {filteredStudents.Count}):");
            foreach (var st in filteredStudents) Console.WriteLine($"   {st}");

            Console.WriteLine("\n=== Завдання 2: Map ===");

            var mappedNumbers = Algorithms.Map(numbers, x => $"({x})");
            Console.WriteLine($"Числа у дужках: {string.Join(" ", mappedNumbers)}");

            var mappedWords = Algorithms.Map(words, w => CountVowels(w, vowels));
            Console.WriteLine($"Кількість голосних у словах: {string.Join(", ", mappedWords)}");

            var mappedStudents = Algorithms.Map(students, s => $"{s.Name}: {(s.Score >= 60 ? "Склав" : "Не склав")}");
            Console.WriteLine("Результати складання іспиту:");
            foreach (var res in mappedStudents) Console.WriteLine($"   {res}");

            Console.WriteLine("\n=== Завдання 3: Aggregate (на наборі чисел) ===");

            int sum = Algorithms.Aggregate(numbers, 0, (acc, x) => acc + x);
            Console.WriteLine($"1. Сума: {sum}");

            long product = Algorithms.Aggregate(numbers, 1L, (acc, x) => acc * x);
            Console.WriteLine($"2. Добуток: {product}");

            int max = Algorithms.Aggregate(numbers, int.MinValue, (acc, x) => x > acc ? x : acc);
            Console.WriteLine($"3. Максимум: {max}");

            string concatenated = Algorithms.Aggregate(words, "", (acc, w) => string.IsNullOrEmpty(acc) ? w : acc + ", " + w);
            Console.WriteLine($"4. Конкатенація рядків: {concatenated}");

            Console.WriteLine("\n=== Завдання 4: GroupBy ===");

            Console.WriteLine("1. Студенти за групою:");
            var byGroup = Algorithms.GroupBy(students, s => s.Group);
            foreach (var kvp in byGroup)
                Console.WriteLine($"   Група '{kvp.Key}': {kvp.Value.Count} елементів");

            Console.WriteLine("2. Студенти за першою літерою прізвища:");
            var byFirstLetter = Algorithms.GroupBy(students, s => s.Name[0]);
            foreach (var kvp in byFirstLetter)
                Console.WriteLine($"   Буква '{kvp.Key}': {kvp.Value.Count} елементів");

            Console.WriteLine("3. Студенти за діапазоном балів:");
            var byScoreRange = Algorithms.GroupBy(students, s => GetScoreRange(s.Score));
            foreach (var kvp in byScoreRange)
                Console.WriteLine($"   Діапазон '{kvp.Key}': {kvp.Value.Count} елементів");

            Console.WriteLine("4. Рядки за довжиною:");
            var byLength = Algorithms.GroupBy(words, w => w.Length);
            foreach (var kvp in byLength)
                Console.WriteLine($"   Довжина {kvp.Key}: {kvp.Value.Count} елементів");

            Console.WriteLine("\n=== Завдання 5: Захоплення змінних ===");
            DemonstrateCapture(K);

            Console.WriteLine("\n=== Перевірка крайових випадків ===");
            TestEdgeCases();
        }

        static List<int> GenerateNumbers(int N, int K)
        {
            Random rnd = new Random(N);
            List<int> list = new List<int>();
            for (int i = 0; i < 20; i++)
            {
                list.Add(rnd.Next(-K, K + 1));
            }
            return list;
        }

        static List<string> GetWords()
        {
            return new List<string>
            {
                "алгоритм", "аналіз", "код", "C#", "делегат", "IT-коледж",
                "яблуко", "огірок", "інтерфейс", "веб-розробка", "екзамен", "авто"
            };
        }

        static List<Student> GetStudents()
        {
            return new List<Student>
            {
                new Student("Іванов", "КН-3/1", 85),
                new Student("Петров", "КН-3/1", 55),
                new Student("Сидоров", "КН-3/2", 72),
                new Student("Коваль", "КН-3/1", 95),
                new Student("Бойко", "КН-3/2", 40),
                new Student("Ткаченко", "ПІ-3/1", 68)
            };
        }

        static int CountVowels(string s, char[] vowels)
        {
            int count = 0;
            foreach (char c in s)
            {
                if (Array.IndexOf(vowels, c) >= 0) count++;
            }
            return count;
        }

        static string GetScoreRange(int score)
        {
            if (score >= 90) return "90–100";
            if (score >= 75) return "75–89";
            if (score >= 60) return "60–74";
            return "0–59";
        }

        static void DemonstrateCapture(int initialK)
        {
            Console.WriteLine("1. Змінна після лямбди:");
            int threshold = initialK;
            Func<int, bool> check = x => x > threshold;
            threshold = 1000;
            Console.WriteLine($"   Виклик check(50) після зміни threshold на 1000: {check(50)}");

            Console.WriteLine("2. Лічильник циклу for:");
            List<Func<int>> actionsFor = new List<Func<int>>();
            for (int i = 0; i < 3; i++)
            {
                actionsFor.Add(() => i);
            }
            Console.Write("   Результати виклику лямбд з for: ");
            foreach (var act in actionsFor)
            {
                Console.Write(act() + " ");
            }
            Console.WriteLine();

            Console.WriteLine("3. Цикл foreach по { 0, 1, 2 }:");
            List<Func<int>> actionsForeach = new List<Func<int>>();
            int[] arr = { 0, 1, 2 };
            foreach (var item in arr)
            {
                actionsForeach.Add(() => item);
            }
            Console.Write("   Результати виклику лямбд з foreach: ");
            foreach (var act in actionsForeach)
            {
                Console.Write(act() + " ");
            }
            Console.WriteLine();
        }

        static void TestEdgeCases()
        {
            List<int> emptyList = new List<int>();

            var fEmpty = Algorithms.Filter(emptyList, x => x > 0);
            Console.WriteLine($"Filter (порожній список): повернуто {fEmpty.Count} елементів.");

            var mEmpty = Algorithms.Map(emptyList, x => x.ToString());
            Console.WriteLine($"Map (порожній список): повернуто {mEmpty.Count} елементів.");

            var gEmpty = Algorithms.GroupBy(emptyList, x => x);
            Console.WriteLine($"GroupBy (порожній список): повернуто {gEmpty.Count} груп.");

            int maxEmpty = Algorithms.Aggregate(emptyList, int.MinValue, (acc, x) => x > acc ? x : acc);
            Console.WriteLine($"Aggregate Максимум (порожній список): повернуто {maxEmpty} (значення start).");

            List<int> numbers = new List<int> { 1, 2, 3 };
            var fNone = Algorithms.Filter(numbers, x => x > 100);
            Console.WriteLine($"Filter (жоден не підходить): повернуто {fNone.Count} елементів.");

            var gAllInOne = Algorithms.GroupBy(numbers, x => "Однорідна група");
            Console.WriteLine($"GroupBy (усі в одній групі): повернуто {gAllInOne.Count} група(и).");

            List<int> singleList = new List<int> { 42 };
            int sumSingle = Algorithms.Aggregate(singleList, 0, (acc, x) => acc + x);
            Console.WriteLine($"Aggregate Сума (список з 1 елемента = 42): повернуто {sumSingle}.");
        }
    }
}