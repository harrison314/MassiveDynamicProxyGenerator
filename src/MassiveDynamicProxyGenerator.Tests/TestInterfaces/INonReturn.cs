using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Tests.TestInterfaces
{
    public interface INonReturn
    {
        void EmptyMethod();

        void OneArgument(int a);

        void OneArgument(string a);

        void OneArgument(StringBuilder a);

        void TwoArguments(int a, StringBuilder sb);

        void MoreArguments(string a, Exception parentEx, StringBuilder builder, Func<Exception, string> transformation);
    }
}
