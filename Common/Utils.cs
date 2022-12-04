using System.Collections;

namespace Common;

public static class Utils
{
    public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, Func<T, bool> selector)
    {
        var result = new List<T>();
        foreach (var item in source)
        {
            if (!selector(item))
            {
                result.Add(item);
            }
            else
            {
                yield return result;
                result = new List<T>();
            }
        }

        yield return result;
    }

    public static string CommonChars(string a, string b)
    {
        return new string(a.ToCharArray().Intersect(b.ToCharArray()).ToArray());
    }
}

public class TextFile : IEnumerable<string>
{
    readonly string _path;

    public TextFile(string path)
    {
        _path = path;
    }

    public IEnumerator<string> GetEnumerator()
    {
        using var read = File.OpenText(_path);

        string? line;
        while ((line = read.ReadLine()) != null)
        {
            yield return line;
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}