using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shouldly
{
    public static class ExceptionAssertion
    {
        public static void SouldException<TException>(Action action)
            where TException : Exception
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            try
            {
                action.Invoke();
                Assert.Fail($"Code no throw expcetion {typeof(TException).FullName}.");
            }
            catch (TException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Excepted exception: '{ex.Message}'");
            }
        }

        public static async Task SouldExceptionAsync<TException>(Func<Task> action)
            where TException : Exception
        {
            if (action == null) throw new ArgumentNullException(nameof(action));

            try
            {
                await action.Invoke();
                Assert.Fail($"Code no throw expcetion {typeof(TException).FullName}.");
            }
            catch (TException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Excepted exception: '{ex.Message}'");
            }
        }

        public static void SouldException<TException>(Action action, Predicate<TException> predicate)
            where TException : Exception
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            try
            {
                action.Invoke();
                Assert.Fail($"Code no throw expcetion {typeof(TException).FullName}.");
            }
            catch (TException ex)
            {
                if (!predicate.Invoke(ex))
                {
                    Assert.Fail($"Code no throw expcetion {typeof(TException).FullName}, but not match predicate.");
                }

                System.Diagnostics.Debug.WriteLine($"Excepted exception: '{ex.Message}'");
            }
        }

        public static async Task SouldExceptionAsync<TException>(Func<Task> action, Predicate<TException> predicate)
           where TException : Exception
        {
            if (action == null) throw new ArgumentNullException(nameof(action));
            if (predicate == null) throw new ArgumentNullException(nameof(predicate));

            try
            {
                await action.Invoke();
                Assert.Fail($"Code no throw expcetion {typeof(TException).FullName}.");
            }
            catch (TException ex)
            {
                if (!predicate.Invoke(ex))
                {
                    Assert.Fail($"Code no throw expcetion {typeof(TException).FullName}, but not match predicate.");
                }

                System.Diagnostics.Debug.WriteLine($"Excepted exception: '{ex.Message}'");
            }
        }
    }
}
