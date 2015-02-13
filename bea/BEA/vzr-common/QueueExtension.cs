using System.Collections.Generic;

namespace vzr_common
{
    public static class QueueExtension
    {
        public static Queue<T> Clone<T>(this Queue<T> source)
        {
            var result = new Queue<T>();
            foreach (T src in source)
            {
                result.Enqueue(src);
            }
            return result;
        }

        public static Stack<T> ToStack<T>(this Queue<T> source)
        {
            var result = new Stack<T>();
            foreach (T src in source)
            {
                result.Push(src);
            }
            return result;
        }
    }
}