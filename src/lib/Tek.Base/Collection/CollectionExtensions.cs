namespace Tek.Base
{
    public static class CollectionExtensions
    {
        public static bool IsEmpty<T>(this T[] array)
            => array == null || array.Length == 0;

        public static bool IsNotEmpty<T>(this T[] array)
            => array != null && array.Length > 0;

        public static T[] NullIfEmpty<T>(this T[] array)
            => array.IsEmpty() ? null : array;
    }
}