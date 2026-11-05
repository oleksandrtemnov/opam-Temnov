using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lab06
{
    public class Student
    {
        public string Name;
        public string Group;
        public int Score;

        public Student(string name, string group, int score)
        {
            Name = name;
            Group = group;
            Score = score;
        }
    }

    public class Task1Result
    {
        public Dictionary<char, int> Frequencies;
        public int DistinctCount;

        public Task1Result(Dictionary<char, int> frequencies, int distinctCount)
        {
            Frequencies = frequencies;
            DistinctCount = distinctCount;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Завдання 1: Частоти літер ===");
            RunTask1();

            Console.WriteLine("\n=== Завдання 2: Групування за навчальною групою ===");
            RunTask2();

            Console.WriteLine("\n=== Завдання 3: Вилучення дублікатів ===");
            RunTask3();

            Console.WriteLine("\n=== Завдання 4: Спільні елементи обох наборів ===");
            RunTask4();

            Console.WriteLine("\n=== Завдання 5: Вимірювання продуктивності ===");
            RunTask5();

            Console.WriteLine("\n=== Крайові випадки ===");
            RunEdgeCases();
        }

        public static Task1Result CountLettersDict(string text)
        {
            Dictionary<char, int> dict = new Dictionary<char, int>();
            int distinct = 0;

            if (text == null)
                return new Task1Result(dict, 0);

            for (int i = 0; i < text.Length; i++)
            {
                char ch = char.ToLower(text[i]);
                if (char.IsLetter(ch))
                {
                    if (dict.ContainsKey(ch))
                    {
                        dict[ch] = dict[ch] + 1;
                    }
                    else
                    {
                        dict.Add(ch, 1);
                        distinct++;
                    }
                }
            }

            return new Task1Result(dict, distinct);
        }

        private static void RunTask1()
        {
            string text = "Алгоритм описує послідовність дій. Алгоритм можна записати словами, а можна записати кодом. Кодом алгоритм стає тоді, коли його виконує машина, а не людина. Одна задача — багато алгоритмів.";
            int threshold = 19 % 3 + 2;

            Task1Result result = CountLettersDict(text);

            Console.WriteLine("Загальна кількість різних літер: " + result.DistinctCount);
            Console.WriteLine("Літери з частотою не менше " + threshold + ":");

            foreach (KeyValuePair<char, int> pair in result.Frequencies)
            {
                if (pair.Value >= threshold)
                {
                    Console.WriteLine("  '" + pair.Key + "': " + pair.Value);
                }
            }
        }

        public static Dictionary<string, List<Student>> GroupStudents(List<Student> students)
        {
            Dictionary<string, List<Student>> map = new Dictionary<string, List<Student>>();

            for (int i = 0; i < students.Count; i++)
            {
                Student s = students[i];
                string key = s.Group;

                if (!map.ContainsKey(key))
                {
                    map.Add(key, new List<Student>());
                }
                map[key].Add(s);
            }

            return map;
        }

        private static void RunTask2()
        {
            List<Student> students = new List<Student>();
            students.Add(new Student("Коваленко", "КН-3/1", 88));
            students.Add(new Student("Мельник", "КН-3/2", 64));
            students.Add(new Student("Іщенко", "КН-3/1", 93));
            students.Add(new Student("Бондар", "КН-3/1", 71));
            students.Add(new Student("Савченко", "КН-3/2", 55));
            students.Add(new Student("Ткачук", "КН-3/2", 90));
            students.Add(new Student("Кравець", "КН-3/1", 76));
            students.Add(new Student("Іванів", "КН-3/1", 48));
            students.Add(new Student("Мороз", "КН-3/2", 82));
            students.Add(new Student("Шевчук", "КН-3/1", 67));
            students.Add(new Student("Бойко", "КН-3/2", 95));
            students.Add(new Student("Кушнір", "КН-3/2", 73));

            Dictionary<string, List<Student>> grouped = GroupStudents(students);

            foreach (KeyValuePair<string, List<Student>> entry in grouped)
            {
                int count = entry.Value.Count;
                double sum = 0;
                for (int i = 0; i < count; i++)
                {
                    sum += entry.Value[i].Score;
                }
                double avg = sum / count;

                Console.WriteLine("Група: " + entry.Key + " | Кількість: " + count + " | Середній бал: " + avg.ToString("F2"));
            }
        }

        public static List<int> RemoveDuplicatesHashSet(List<int> input)
        {
            HashSet<int> set = new HashSet<int>(input);
            return new List<int>(set);
        }

        public static List<int> RemoveDuplicatesOrdered(List<int> input)
        {
            HashSet<int> seen = new HashSet<int>();
            List<int> result = new List<int>();

            for (int i = 0; i < input.Count; i++)
            {
                if (seen.Add(input[i]))
                {
                    result.Add(input[i]);
                }
            }

            return result;
        }

        private static void RunTask3()
        {
            Random rnd = new Random(25);
            List<int> numbers = new List<int>();
            int maxVal = 19 % 10 + 6;

            for (int i = 0; i < 25; i++)
            {
                numbers.Add(rnd.Next(1, maxVal + 1));
            }

            Console.WriteLine("Початковий список: " + string.Join(", ", numbers));

            List<int> resUnordered = RemoveDuplicatesHashSet(numbers);
            Console.WriteLine("Без збереження порядку (HashSet): " + string.Join(", ", resUnordered));

            List<int> resOrdered = RemoveDuplicatesOrdered(numbers);
            Console.WriteLine("Зі збереженням порядку (HashSet + List): " + string.Join(", ", resOrdered));
        }

        public static List<int> CommonSet(List<int> list1, List<int> list2)
        {
            HashSet<int> set1 = new HashSet<int>(list1);
            HashSet<int> set2 = new HashSet<int>(list2);

            set1.IntersectWith(set2);
            return new List<int>(set1);
        }

        public static List<int> CommonList(List<int> list1, List<int> list2)
        {
            List<int> result = new List<int>();

            for (int i = 0; i < list1.Count; i++)
            {
                int item = list1[i];
                bool inSecond = false;

                for (int j = 0; j < list2.Count; j++)
                {
                    if (list2[j] == item)
                    {
                        inSecond = true;
                        break;
                    }
                }

                if (inSecond)
                {
                    bool alreadyAdded = false;
                    for (int k = 0; k < result.Count; k++)
                    {
                        if (result[k] == item)
                        {
                            alreadyAdded = true;
                            break;
                        }
                    }

                    if (!alreadyAdded)
                    {
                        result.Add(item);
                    }
                }
            }

            return result;
        }

        private static void RunTask4()
        {
            Random rnd1 = new Random(25);
            Random rnd2 = new Random(125);

            List<int> set1 = new List<int>();
            List<int> set2 = new List<int>();

            int maxVal = 20 + 19;

            for (int i = 0; i < 30; i++)
            {
                set1.Add(rnd1.Next(1, maxVal + 1));
                set2.Add(rnd2.Next(1, maxVal + 1));
            }

            List<int> resSet = CommonSet(set1, set2);
            List<int> resList = CommonList(set1, set2);

            resSet.Sort();
            resList.Sort();

            Console.WriteLine("Спільні елементи (через HashSet): " + string.Join(", ", resSet) + " (Кількість: " + resSet.Count + ")");
            Console.WriteLine("Спільні елементи (через List):    " + string.Join(", ", resList) + " (Кількість: " + resList.Count + ")");

            bool equal = resSet.Count == resList.Count;
            if (equal)
            {
                for (int i = 0; i < resSet.Count; i++)
                {
                    if (resSet[i] != resList[i])
                    {
                        equal = false;
                        break;
                    }
                }
            }

            Console.WriteLine("Результати ідентичні: " + equal);
        }

        private static void RunTask5()
        {
            Stopwatch sw = new Stopwatch();

            List<int> list = new List<int>();
            for (int i = 0; i < 10000; i++) list.Add(i);
            HashSet<int> set = new HashSet<int>(list);

            sw.Restart();
            for (int i = 0; i < 1000; i++) list.Contains(i * 2);
            sw.Stop();
            double t_list = sw.Elapsed.TotalMilliseconds;

            sw.Restart();
            for (int i = 0; i < 1000; i++) set.Contains(i * 2);
            sw.Stop();
            double t_set = sw.Elapsed.TotalMilliseconds;

            double ratio = t_list / Math.Max(t_set, 0.001);
            Console.WriteLine("Замір (Перевірка наявності): Список = " + t_list.ToString("F2") + " ms, HashSet = " + t_set.ToString("F2") + " ms, Виграш = " + ratio.ToString("F1") + "x");
        }

        private static void RunEdgeCases()
        {
            Task1Result z1_empty = CountLettersDict("");
            Console.WriteLine("Завдання 1 (порожній текст): Кількість літер = " + z1_empty.DistinctCount);

            Task1Result z1_unique = CountLettersDict("абвгґд");
            Console.WriteLine("Завдання 1 (усі літери унікальні): Кількість літер = " + z1_unique.DistinctCount);

            List<Student> sameGroup = new List<Student>();
            sameGroup.Add(new Student("Іваненко", "КН-3/1", 90));
            sameGroup.Add(new Student("Петренко", "КН-3/1", 80));

            Dictionary<string, List<Student>> z2_same = GroupStudents(sameGroup);
            Console.WriteLine("Завдання 2 (усі в одній групі): Груп у словнику = " + z2_same.Count);

            List<int> noDups = new List<int>();
            noDups.Add(1);
            noDups.Add(2);
            noDups.Add(3);

            List<int> z3_res = RemoveDuplicatesHashSet(noDups);
            Console.WriteLine("Завдання 3 (без дублікатів): Довжина після вилучення = " + z3_res.Count);

            List<int> listA = new List<int>();
            listA.Add(1);
            listA.Add(2);

            List<int> listB = new List<int>();
            listB.Add(3);
            listB.Add(4);

            List<int> z4_noIntersect = CommonSet(listA, listB);
            Console.WriteLine("Завдання 4 (не перетинаються): Спільних елементів = " + z4_noIntersect.Count);
        }
    }
}