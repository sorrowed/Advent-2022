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
        return new string((a.ToCharArray().Intersect(b.ToCharArray())).ToArray());
    }

}