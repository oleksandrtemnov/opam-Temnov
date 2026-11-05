using System;
using System.Collections.Generic;

namespace Lab05
{
    public class CharStack
    {
        private char[] _items;
        private int _count;

        public int Count
        {
            get { return _count; }
        }

        public int Capacity
        {
            get { return _items.Length; }
        }

        public CharStack(int initialCapacity)
        {
            if (initialCapacity <= 0)
                initialCapacity = 5;
            _items = new char[initialCapacity];
            _count = 0;
        }

        public void Push(char item)
        {
            if (_count == _items.Length)
            {
                char[] newArray = new char[_items.Length * 2];
                Array.Copy(_items, newArray, _count);
                _items = newArray;
            }
            _items[_count] = item;
            _count++;
        }

        public char Pop()
        {
            if (_count == 0)
                throw new InvalidOperationException("Стек порожній!");
            _count--;
            return _items[_count];
        }

        public char Peek()
        {
            if (_count == 0)
                throw new InvalidOperationException("Стек порожній!");
            return _items[_count - 1];
        }
    }

    public class IntStack
    {
        private int[] _items;
        private int _count;

        public int Count
        {
            get { return _count; }
        }

        public int Capacity
        {
            get { return _items.Length; }
        }

        public IntStack(int initialCapacity)
        {
            if (initialCapacity <= 0)
                initialCapacity = 5;
            _items = new int[initialCapacity];
            _count = 0;
        }

        public void Push(int item)
        {
            if (_count == _items.Length)
            {
                int[] newArray = new int[_items.Length * 2];
                Array.Copy(_items, newArray, _count);
                _items = newArray;
            }
            _items[_count] = item;
            _count++;
        }

        public int Pop()
        {
            if (_count == 0)
                throw new InvalidOperationException("Стек порожній!");
            _count--;
            return _items[_count];
        }

        public int Peek()
        {
            if (_count == 0)
                throw new InvalidOperationException("Стек порожній!");
            return _items[_count - 1];
        }
    }

    public class BracketResult
    {
        public bool IsBalanced;
        public int ErrorIndex;

        public BracketResult(bool isBalanced, int errorIndex)
        {
            IsBalanced = isBalanced;
            ErrorIndex = errorIndex;
        }
    }

    public class RpnResult
    {
        public bool Success;
        public double Result;
        public string ErrorMessage;

        public RpnResult(bool success, double result, string errorMessage)
        {
            Success = success;
            Result = result;
            ErrorMessage = errorMessage;
        }
    }

    public class BracketItem
    {
        public char Symbol;
        public int Index;

        public BracketItem(char symbol, int index)
        {
            Symbol = symbol;
            Index = index;
        }
    }

    public class RequestItem
    {
        public int Id;
        public int Weight;

        public RequestItem(int id, int weight)
        {
            Id = id;
            Weight = weight;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("=== Завдання 1: Збалансованість дужок ===");
            RunTask1StandardTests();

            Console.WriteLine("\n=== Завдання 2: Зворотний польський запис (RPN) ===");
            RunTask2Tests();

            Console.WriteLine("\n=== Завдання 3: Модель черги обслуговування ===");
            RunTask3Simulation();

            Console.WriteLine("\n=== Завдання 4: Перевірка власного стеку ===");
            RunTask4Tests();

            Console.WriteLine("\n=== Перевірка крайових випадків ===");
            RunEdgeCases();
        }

        public static BracketResult CheckBracketsStandard(string input)
        {
            if (input == null)
                return new BracketResult(true, -1);

            Stack<BracketItem> stack = new Stack<BracketItem>();

            for (int i = 0; i < input.Length; i++)
            {
                char ch = input[i];

                if (IsOpeningBracket(ch))
                {
                    stack.Push(new BracketItem(ch, i));
                }
                else if (IsClosingBracket(ch))
                {
                    if (stack.Count == 0)
                    {
                        return new BracketResult(false, i);
                    }

                    BracketItem top = stack.Pop();
                    if (!IsMatchingPair(top.Symbol, ch))
                    {
                        return new BracketResult(false, i);
                    }
                }
            }

            if (stack.Count > 0)
            {
                BracketItem top = stack.Pop();
                return new BracketResult(false, top.Index);
            }

            return new BracketResult(true, -1);
        }

        public static BracketResult CheckBracketsCustom(string input)
        {
            if (input == null)
                return new BracketResult(true, -1);

            CharStack charStack = new CharStack(5);
            IntStack indexStack = new IntStack(5);

            for (int i = 0; i < input.Length; i++)
            {
                char ch = input[i];

                if (IsOpeningBracket(ch))
                {
                    charStack.Push(ch);
                    indexStack.Push(i);
                }
                else if (IsClosingBracket(ch))
                {
                    if (charStack.Count == 0)
                    {
                        return new BracketResult(false, i);
                    }

                    char topChar = charStack.Pop();
                    indexStack.Pop();

                    if (!IsMatchingPair(topChar, ch))
                    {
                        return new BracketResult(false, i);
                    }
                }
            }

            if (charStack.Count > 0)
            {
                charStack.Pop();
                int topIndex = indexStack.Pop();
                return new BracketResult(false, topIndex);
            }

            return new BracketResult(true, -1);
        }

        private static bool IsOpeningBracket(char c)
        {
            return c == '(' || c == '[' || c == '{' || c == '<';
        }

        private static bool IsClosingBracket(char c)
        {
            return c == ')' || c == ']' || c == '}' || c == '>';
        }

        private static bool IsMatchingPair(char open, char close)
        {
            if (open == '(' && close == ')') return true;
            if (open == '[' && close == ']') return true;
            if (open == '{' && close == '}') return true;
            if (open == '<' && close == '>') return true;
            return false;
        }

        private static void RunTask1StandardTests()
        {
            string[] tests = new string[]
            {
                "(a[b]{c})",
                "([)]",
                ")(",
                "((",
                ""
            };

            string customTest = new string('(', 19) + new string(')', 18);

            for (int i = 0; i < tests.Length; i++)
            {
                string test = tests[i];
                BracketResult res = CheckBracketsStandard(test);
                Console.WriteLine("Рядок: \"" + test + "\" -> Збалансований: " + res.IsBalanced + ", Індекс помилки: " + res.ErrorIndex);
            }

            BracketResult customRes = CheckBracketsStandard(customTest);
            Console.WriteLine("\nСпеціальний тест із K=19 '(' та K-1=18 ')':");
            Console.WriteLine("Рядок: \"" + customTest + "\"");
            Console.WriteLine("Результат: Збалансований = " + customRes.IsBalanced + ", Індекс помилки = " + customRes.ErrorIndex);
        }

        public static RpnResult EvaluateRPN(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression))
            {
                return new RpnResult(false, 0, "Вираз порожній.");
            }

            string[] tokens = expression.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Stack<double> stack = new Stack<double>();

            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];
                double number;

                if (double.TryParse(token, out number))
                {
                    stack.Push(number);
                }
                else if (IsOperator(token))
                {
                    if (stack.Count < 2)
                    {
                        return new RpnResult(false, 0, "Забракло чисел для операції '" + token + "'.");
                    }

                    double b = stack.Pop();
                    double a = stack.Pop();

                    if (token == "+")
                    {
                        stack.Push(a + b);
                    }
                    else if (token == "-")
                    {
                        stack.Push(a - b);
                    }
                    else if (token == "*")
                    {
                        stack.Push(a * b);
                    }
                    else if (token == "/")
                    {
                        if (Math.Abs(b) < 1e-9)
                            return new RpnResult(false, 0, "Помилка: ділення на нуль.");
                        stack.Push(a / b);
                    }
                    else if (token == "%")
                    {
                        if (Math.Abs(b) < 1e-9)
                            return new RpnResult(false, 0, "Помилка: остача від ділення на нуль.");
                        stack.Push(a % b);
                    }
                }
                else
                {
                    return new RpnResult(false, 0, "Невідомий токен: '" + token + "'.");
                }
            }

            if (stack.Count == 1)
            {
                return new RpnResult(true, stack.Pop(), null);
            }
            else
            {
                return new RpnResult(false, 0, "Помилка у виразі: забагато чисел.");
            }
        }

        private static bool IsOperator(string token)
        {
            return token == "+" || token == "-" || token == "*" || token == "/" || token == "%";
        }

        private static void RunTask2Tests()
        {
            string[] mandatoryTests = new string[]
            {
                "3 4 + 2 *",
                "5 3 -",
                "8 2 /",
                "2 3 4 * +",
                "5 0 /"
            };

            for (int i = 0; i < mandatoryTests.Length; i++)
            {
                string expr = mandatoryTests[i];
                RpnResult res = EvaluateRPN(expr);
                if (res.Success)
                    Console.WriteLine("\"" + expr + "\" -> " + res.Result);
                else
                    Console.WriteLine("\"" + expr + "\" -> Помилка: " + res.ErrorMessage);
            }

            string customExpr = "40 10 % 20 + 1 -";
            RpnResult customRes = EvaluateRPN(customExpr);
            Console.WriteLine("\nВласний вираз: \"" + customExpr + "\" -> Результат: " + customRes.Result + " (Очікується 19)");
        }

        public static void RunTask3Simulation()
        {
            int requestCount = 12;
            Random rnd = new Random(25);

            Queue<RequestItem> queue = new Queue<RequestItem>();

            for (int i = 1; i <= requestCount; i++)
            {
                int weight = rnd.Next(1, 10);
                queue.Enqueue(new RequestItem(i, weight));
            }

            int maxQueueLength = queue.Count;
            int currentTime = 0;

            Console.WriteLine(string.Format("{0,-10} | {1,-10} | {2,-12} | {3,-14}", "№ Заявки", "Вага (хв)", "Час початку", "Час завершення"));
            Console.WriteLine(new string('-', 55));

            while (queue.Count > 0)
            {
                if (queue.Count > maxQueueLength)
                    maxQueueLength = queue.Count;

                RequestItem currentRequest = queue.Dequeue();
                int startTime = currentTime;
                int endTime = startTime + currentRequest.Weight;
                currentTime = endTime;

                Console.WriteLine(string.Format("{0,-10} | {1,-10} | {2,-12} | {3,-14}", currentRequest.Id, currentRequest.Weight, startTime, endTime));
            }

            Console.WriteLine(new string('-', 55));
            Console.WriteLine("Максимальна довжина черги за час моделювання: " + maxQueueLength);
        }

        private static void RunTask4Tests()
        {
            IntStack stack = new IntStack(5);
            Console.WriteLine("Початковий стан: Count = 0, Capacity = 5");

            for (int i = 1; i <= 6; i++)
            {
                stack.Push(i * 10);
                Console.WriteLine("Push(" + (i * 10) + ") -> Count: " + stack.Count + ", Capacity: " + stack.Capacity);
            }

            Console.WriteLine("\nПеревірка Завдання 1 на власному CharStack:");
            string testStr = "(a[b]{c})";
            BracketResult resCustom = CheckBracketsCustom(testStr);
            BracketResult resStd = CheckBracketsStandard(testStr);
            Console.WriteLine("Текст: \"" + testStr + "\"");
            Console.WriteLine("Результат на CharStack: Balanced=" + resCustom.IsBalanced + ", ErrorIndex=" + resCustom.ErrorIndex);
            Console.WriteLine("Результат на Stack<T>:  Balanced=" + resStd.IsBalanced + ", ErrorIndex=" + resStd.ErrorIndex);
        }

        private static void RunEdgeCases()
        {
            Console.WriteLine("\n--- Перевірка крайових випадків ---");

            BracketResult z1_empty = CheckBracketsStandard("");
            RpnResult z2_empty = EvaluateRPN("");
            Console.WriteLine("Завдання 1 (порожній рядок): Balanced=" + z1_empty.IsBalanced + ", ErrorIndex=" + z1_empty.ErrorIndex);
            Console.WriteLine("Завдання 2 (порожній рядок): Success=" + z2_empty.Success + ", Error=" + z2_empty.ErrorMessage);

            BracketResult z1_noBrackets = CheckBracketsStandard("Hello World 123");
            Console.WriteLine("Завдання 1 (без дужок): Balanced=" + z1_noBrackets.IsBalanced + ", ErrorIndex=" + z1_noBrackets.ErrorIndex);

            RpnResult z2_single = EvaluateRPN("42");
            Console.WriteLine("Завдання 2 (одне число '42'): Success=" + z2_single.Success + ", Result=" + z2_single.Result);

            RpnResult z2_divZero = EvaluateRPN("5 0 /");
            Console.WriteLine("Завдання 2 (ділення на нуль '5 0 /'): Success=" + z2_divZero.Success + ", Error=" + z2_divZero.ErrorMessage);

            Console.WriteLine("Завдання 3 (0 заявок):");
            Queue<RequestItem> emptyQ = new Queue<RequestItem>();
            Console.WriteLine("  Елементів у черзі: " + emptyQ.Count + ", Макс. довжина: " + (emptyQ.Count > 0 ? emptyQ.Count : 0));

            Console.Write("Завдання 4 (Pop на порожньому стеку): ");
            try
            {
                IntStack emptyStack = new IntStack(5);
                emptyStack.Pop();
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Зловлено виняток: " + ex.Message);
            }
        }
    }
}