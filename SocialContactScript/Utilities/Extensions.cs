namespace SocialContactScript.Utilities
{
    using System.Collections.Generic;

    public static class Extensions
    {
        /// <summary>
        /// a simple count forward to drop the first left many elements and the same discarded buffer to drop the last right many elements. 
        /// http://stackoverflow.com/a/4147812/1603970
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<T> Shrink<T>(this IEnumerable<T> source, int left, int right)
        {
            int i = 0;
            var buffer = new Queue<T>(right + 1);

            foreach (var x in source)
            {
                if (i >= left) // Read past left many elements at the start
                {
                    buffer.Enqueue(x);
                    if (buffer.Count > right) // Build a buffer to drop right many elements at the end
                        yield return buffer.Dequeue();
                }
                else i++;
            }
        }
        public static IEnumerable<T> SkipLast<T>(this IEnumerable<T> source, int n = 1)
        {
            return source.Shrink(0, n);
        }
        public static IEnumerable<T> SkipFirst<T>(this IEnumerable<T> source, int n = 1)
        {
            return source.Shrink(n, 0);
        }
    }
}
