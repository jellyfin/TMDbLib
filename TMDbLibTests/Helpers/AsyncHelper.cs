using System.Threading.Tasks;

namespace TMDbLibTests.Helpers
{
    public static class AsyncHelper
    {
        public static void Sync(this Task task)
        {
            task.GetAwaiter().GetResult();
        }

        public static T Sync<T>(this Task<T> task)
        {
            return task.GetAwaiter().GetResult();
        }
    }
}