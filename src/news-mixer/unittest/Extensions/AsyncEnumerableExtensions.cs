namespace NewsMixer.UnitTests.Extensions
{
    internal static class AsyncEnumerableExtensions
    {
        internal static async Task<IList<T>> ToListAsync<T>(this IAsyncEnumerable<T> enumerable)
        {
            var list = new List<T>();
            await foreach(var itm in enumerable)
            {
                list.Add(itm);
            }

            return list;
        }

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
