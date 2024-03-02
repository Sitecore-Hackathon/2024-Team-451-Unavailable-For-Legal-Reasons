namespace NewsMixer.UnitTests.Extensions
{
    internal static class AsyncEnumerableExtensions
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        internal static async IAsyncEnumerable<T> ToAsyncEnumerable<T>(this IEnumerable<T> enumerable)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            foreach(var itm in enumerable)
            {
                yield return itm;
            }
        }
    }
}
