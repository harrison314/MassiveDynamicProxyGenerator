using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Should
{
    public static class TypeAssertExtensions
    {
        public static void ShouldBySameTypeAs<T>(this object instance, T their)
        {
            if (instance == null && their == null)
            {
                return;
            }

            if (instance == null)
            {
                Assert.Fail("Instance is null.");
            }

            if (their == null)
            {
                Assert.Fail("Their is null.");
            }

            if (instance.GetType() != their.GetType())
            {
                Assert.Fail($"Instance type '{instance.GetType().FullName}' is not equal type of their '{their.GetType().FullName}'");
            }
        }

        public static void ShouldNotBySameTypeAs<T>(this object instance, T their)
        {
            if (instance == null && their == null)
            {
                Assert.Fail("Instance and theri is both null.");
            }

            if (instance == null || their == null)
            {
                return;
            }

            if (instance.GetType() == their.GetType())
            {
                Assert.Fail($"Instance and their is same type '{instance.GetType().FullName}'.");
            }
        }
    }
}
