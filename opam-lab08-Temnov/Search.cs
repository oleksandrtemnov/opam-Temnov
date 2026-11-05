using System;

namespace SearchLab
{
    public static class Search
    {
        public static int LinearSearch<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
        {
            comparisons = 0;
            for (int i = 0; i < a.Length; i++)
            {
                comparisons++;
                if (cmp(a[i], target) == 0)
                    return i;
            }
            return -1;
        }

        public static int BinarySearch<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
        {
            comparisons = 0;
            int lo = 0;
            int hi = a.Length - 1;

            while (lo <= hi)
            {
                int mid = lo + (hi - lo) / 2;
                comparisons++;
                int res = cmp(a[mid], target);

                if (res == 0)
                    return mid;
                if (res < 0)
                    lo = mid + 1;
                else
                    hi = mid - 1;
            }

            return -1;
        }

        public static int FindBound<T>(T[] a, Func<T, bool> predicate, out int comparisons)
        {
            comparisons = 0;
            int lo = 0;
            int hi = a.Length - 1;
            int result = a.Length;

            while (lo <= hi)
            {
                int mid = lo + (hi - lo) / 2;
                comparisons++;
                if (predicate(a[mid]))
                {
                    result = mid;
                    hi = mid - 1;
                }
                else
                {
                    lo = mid + 1;
                }
            }

            return result;
        }

        public static int LowerBound<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
        {
            return FindBound(a, item => cmp(item, target) >= 0, out comparisons);
        }

        public static int UpperBound<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
        {
            return FindBound(a, item => cmp(item, target) > 0, out comparisons);
        }

        public static int RecursiveBinarySearch<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons, out int maxDepth)
        {
            comparisons = 0;
            int currentDepth = 0;
            maxDepth = 0;
            return RecursiveSearchInternal(a, target, 0, a.Length - 1, cmp, ref comparisons, currentDepth, ref maxDepth);
        }

        private static int RecursiveSearchInternal<T>(T[] a, T target, int lo, int hi, Func<T, T, int> cmp, ref int comparisons, int depth, ref int maxDepth)
        {
            depth++;
            if (depth > maxDepth)
                maxDepth = depth;

            if (lo > hi)
                return -1;

            int mid = lo + (hi - lo) / 2;
            comparisons++;
            int res = cmp(a[mid], target);

            if (res == 0)
                return mid;
            if (res < 0)
                return RecursiveSearchInternal(a, target, mid + 1, hi, cmp, ref comparisons, depth, ref maxDepth);
            else
                return RecursiveSearchInternal(a, target, lo, mid - 1, cmp, ref comparisons, depth, ref maxDepth);
        }
    }
}