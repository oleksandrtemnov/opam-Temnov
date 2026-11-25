using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Lab10;

public static class StringTasks
{
    public static string Trap1ConcatPlus(int count)
    {
        string s = string.Empty;
        for (int i = 0; i < count; i++)
        {
            s += 'О';
        }
        return s;
    }

    public static string Trap1StringBuilder(int count)
    {
        var sb = new StringBuilder(count);
        for (int i = 0; i < count; i++)
        {
            sb.Append('О');
        }
        return sb.ToString();
    }

    public static int Trap2ToLowerCompare(string[] words, int iterations)
    {
        int equalCount = 0;
        int len = words.Length;
        for (int i = 0; i < iterations; i++)
        {
            string a = words[i % len];
            string b = words[(i + 1) % len];
            if (a.ToLower() == b.ToLower())
            {
                equalCount++;
            }
        }
        return equalCount;
    }

    public static int Trap2OrdinalIgnoreCase(string[] words, int iterations)
    {
        int equalCount = 0;
        int len = words.Length;
        for (int i = 0; i < iterations; i++)
        {
            string a = words[i % len];
            string b = words[(i + 1) % len];
            if (string.Equals(a, b, StringComparison.OrdinalIgnoreCase))
            {
                equalCount++;
            }
        }
        return equalCount;
    }

    public static (string[] OrdinalSorted, string[] CultureSorted) Trap3CompareSorts()
    {
        string[] words =
        {
            "ґанок", "ганок", "ґрунт", "грунт", "ґудзик",
            "гусак", "ґедзь", "герой", "ґрати", "грати",
            "ґава", "гава", "ґлей", "глей", "ґніт",
            "гніт", "ґоґель", "гомін", "ґрунтовний", "гуркіт"
        };

        string[] ordinal = (string[])words.Clone();
        Array.Sort(ordinal, StringComparer.Ordinal);

        string[] culture = (string[])words.Clone();
        Array.Sort(culture, StringComparer.Create(new CultureInfo("uk-UA"), false));

        return (ordinal, culture);
    }

    public static List<string> ExtractWords(string text)
    {
        var words = new List<string>();
        if (string.IsNullOrEmpty(text))
            return words;

        var sb = new StringBuilder();
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            if (char.IsLetter(c))
            {
                sb.Append(c);
            }
            else if (sb.Length > 0)
            {
                words.Add(sb.ToString());
                sb.Clear();
            }
        }

        if (sb.Length > 0)
            words.Add(sb.ToString());

        return words;
    }

    public static List<(string Word, int Count)> TopKWordsNaive(string text, int k)
    {
        List<string> words = ExtractWords(text);
        if (words.Count == 0 || k <= 0)
            return new List<(string, int)>();

        var uniqueCounts = new List<(string Word, int Count)>();

        for (int i = 0; i < words.Count; i++)
        {
            string current = words[i];
            bool alreadyProcessed = false;

            for (int prev = 0; prev < i; prev++)
            {
                if (string.Equals(words[prev], current, StringComparison.OrdinalIgnoreCase))
                {
                    alreadyProcessed = true;
                    break;
                }
            }

            if (alreadyProcessed)
                continue;

            int count = 0;
            for (int j = i; j < words.Count; j++)
            {
                if (string.Equals(words[j], current, StringComparison.OrdinalIgnoreCase))
                {
                    count++;
                }
            }

            uniqueCounts.Add((current.ToLowerInvariant(), count));
        }

        uniqueCounts.Sort((a, b) =>
        {
            int cmp = b.Count.CompareTo(a.Count);
            if (cmp != 0) return cmp;
            return string.Compare(a.Word, b.Word, StringComparison.Ordinal);
        });

        int take = Math.Min(k, uniqueCounts.Count);
        var result = new List<(string, int)>(take);
        for (int i = 0; i < take; i++)
            result.Add(uniqueCounts[i]);

        return result;
    }

    public static List<(string Word, int Count)> TopKWordsFast(string text, int k)
    {
        List<string> words = ExtractWords(text);
        if (words.Count == 0 || k <= 0)
            return new List<(string, int)>();

        var freq = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        for (int i = 0; i < words.Count; i++)
        {
            string w = words[i];
            if (freq.TryGetValue(w, out int count))
                freq[w] = count + 1;
            else
                freq[w] = 1;
        }

        var list = new List<(string Word, int Count)>(freq.Count);
        foreach (var kvp in freq)
        {
            list.Add((kvp.Key.ToLowerInvariant(), kvp.Value));
        }

        list.Sort((a, b) =>
        {
            int cmp = b.Count.CompareTo(a.Count);
            if (cmp != 0) return cmp;
            return string.Compare(a.Word, b.Word, StringComparison.Ordinal);
        });

        int take = Math.Min(k, list.Count);
        var result = new List<(string, int)>(take);
        for (int i = 0; i < take; i++)
            result.Add(list[i]);

        return result;
    }

    public static bool AreTopKEqual(List<(string Word, int Count)> a, List<(string Word, int Count)> b)
    {
        if (a.Count != b.Count)
            return false;

        for (int i = 0; i < a.Count; i++)
        {
            if (a[i].Count != b[i].Count || !string.Equals(a[i].Word, b[i].Word, StringComparison.Ordinal))
                return false;
        }
        return true;
    }

    public static int NaiveIndexOf(string text, string pat, out int comparisons)
    {
        comparisons = 0;
        if (pat.Length == 0)
            return 0;
        if (pat.Length > text.Length)
            return -1;

        int limit = text.Length - pat.Length;
        for (int i = 0; i <= limit; i++)
        {
            int j = 0;
            while (j < pat.Length)
            {
                comparisons++;
                if (text[i + j] != pat[j])
                    break;
                j++;
            }

            if (j == pat.Length)
                return i;
        }

        return -1;
    }
}