namespace NewsMixer.Extensions
{
    public static class AsyncEnumerableExtensions
    {
        public static async Task<IList<T>> ToListAsync<T>(this IAsyncEnumerable<T> enumerable)
        {
            var list = new List<T>();
            await foreach (var itm in enumerable)
            {
                list.Add(itm);
            }

            return list;
        }
    }
}
