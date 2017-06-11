using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WcfForHipsters.WebServer.WcfForHipsters
{
    internal static class UnwrapHelper
    {
        public static object UnwrapTask(Task t)
        {
            t.GetAwaiter().GetResult();

            return null;
        }

        public static object UnwrapGenericTask<T>(Task<T> task)
        {
            return task.GetAwaiter().GetResult();
        }
    }
}
