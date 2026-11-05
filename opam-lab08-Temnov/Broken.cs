using System;

namespace SearchLab
{
    public static class Broken
    {
        public static int Broken1_Condition<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
        {
            comparisons = 0;
            int lo = 0;
            int hi = a.Length - 1;

            while (lo < hi)
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

        public static int Broken2_InfiniteLoop<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons, out bool hanged)
        {
            comparisons = 0;
            hanged = false;
            int lo = 0;
            int hi = a.Length - 1;
            int steps = 0;

            while (lo <= hi)
            {
                steps++;
                if (steps > 100)
                {
                    hanged = true;
                    return -1;
                }

                int mid = lo + (hi - lo) / 2;
                comparisons++;
                int res = cmp(a[mid], target);

                if (res == 0)
                    return mid;
                if (res < 0)
                    lo = mid;
                else
                    hi = mid - 1;
            }

            return -1;
        }

        public static int Broken3_Overflow<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
        {
            comparisons = 0;
            int lo = 0;
            int hi = a.Length - 1;

            while (lo <= hi)
            {
                int mid = (lo + hi) / 2;
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

        public static int Broken4_UnsortedSearch<T>(T[] unsortedArray, T target, Func<T, T, int> cmp, out int comparisons)
        {
            return Search.BinarySearch(unsortedArray, target, cmp, out comparisons);
        }
    }
}