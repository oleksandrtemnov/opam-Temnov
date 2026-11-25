using System;

namespace Lab09;

public static class Sorts
{
    public static void InsertionSort<T>(T[] a, Func<T, T, int> cmp, out int comparisons, out int moves)
    {
        comparisons = 0;
        moves = 0;
        int n = a.Length;

        for (int i = 1; i < n; i++)
        {
            T key = a[i];
            int j = i - 1;

            while (j >= 0)
            {
                comparisons++;
                if (cmp(a[j], key) > 0)
                {
                    a[j + 1] = a[j];
                    moves++;
                    j--;
                }
                else
                {
                    break;
                }
            }

            a[j + 1] = key;
            moves++;
        }
    }

    public static void BottomUpMergeSort<T>(T[] a, Func<T, T, int> cmp, out int comparisons, out int moves)
    {
        comparisons = 0;
        moves = 0;
        int n = a.Length;
        if (n <= 1) return;

        T[] aux = new T[n];
        T[] src = a;
        T[] dst = aux;

        for (int width = 1; width < n; width *= 2)
        {
            for (int left = 0; left < n; left += 2 * width)
            {
                int mid = Math.Min(left + width, n);
                int right = Math.Min(left + 2 * width, n);

                int i = left;
                int j = mid;
                int k = left;

                while (i < mid && j < right)
                {
                    comparisons++;
                    if (cmp(src[i], src[j]) <= 0)
                    {
                        dst[k++] = src[i++];
                    }
                    else
                    {
                        dst[k++] = src[j++];
                    }
                    moves++;
                }

                while (i < mid)
                {
                    dst[k++] = src[i++];
                    moves++;
                }

                while (j < right)
                {
                    dst[k++] = src[j++];
                    moves++;
                }
            }

            (src, dst) = (dst, src);
        }

        if (!ReferenceEquals(src, a))
        {
            for (int i = 0; i < n; i++)
            {
                a[i] = src[i];
                moves++;
            }
        }
    }

    public static void CountingSort<T>(T[] a, Func<T, int> keySelector, int minVal, int maxVal, out int comparisons, out int moves)
    {
        comparisons = 0;
        moves = 0;
        int n = a.Length;
        if (n <= 1) return;

        int range = maxVal - minVal + 1;
        int[] count = new int[range];

        for (int i = 0; i < n; i++)
        {
            count[keySelector(a[i]) - minVal]++;
        }

        for (int i = 1; i < range; i++)
        {
            count[i] += count[i - 1];
        }

        T[] output = new T[n];
        for (int i = n - 1; i >= 0; i--)
        {
            int key = keySelector(a[i]) - minVal;
            int pos = --count[key];
            output[pos] = a[i];
            moves++;
        }

        for (int i = 0; i < n; i++)
        {
            a[i] = output[i];
            moves++;
        }
    }

    public static void CountingSortInt(int[] a, Func<int, int, int> cmp, out int comparisons, out int moves)
    {
        CountingSort(a, x => x, 0, 380, out comparisons, out moves);
    }
}