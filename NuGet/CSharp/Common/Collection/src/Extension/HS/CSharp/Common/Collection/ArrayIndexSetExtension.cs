using HS.CSharp.Common.Collection;

namespace HS.CSharp.Common.Extension.HS.CSharp.Common.Collection;

public static class ArrayIndexSetExtension
{
    public static ArrayIndexSet<T> ToArrayIndexSet<T>(this T[] array)
    {
        ArrayIndexSet<T> result = new(array, array.Length);
        for (int i = 0; i < array.Length; i++)
        {
            result.Add(i);
        }

        return result;
    }
}