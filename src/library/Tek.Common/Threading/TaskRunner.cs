using System;
using System.Threading.Tasks;

namespace Tek.Common
{
    public static class TaskRunner
    {
        public static void RunSync(Func<Task> action)
        {
            Task.Run(action)
                .GetAwaiter()
                .GetResult();
        }

        public static T RunSync<T>(Func<Task<T>> action)
        {
            return Task.Run(action)
                .GetAwaiter()
                .GetResult();
        }
    }
}
