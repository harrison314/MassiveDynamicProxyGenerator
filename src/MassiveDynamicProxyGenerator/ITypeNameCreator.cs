using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    public interface ITypeNameCreator
    {
        string CreateTypeName();

        string CreateTypeName(string prefix, int lenght);

        string CreateMethodName();

        string CreateMethodName(string prefix, int lenght);
    }
}
