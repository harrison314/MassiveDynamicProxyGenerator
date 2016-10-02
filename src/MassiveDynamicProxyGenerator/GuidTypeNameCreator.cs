using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    public class GuidTypeNameCreator : ITypeNameCreator
    {
        public GuidTypeNameCreator()
        {
        }

        public string CreateMethodName()
        {
            string name = string.Concat("M", Guid.NewGuid().ToString("D").Replace("-", string.Empty));

            return name;
        }

        public string CreateMethodName(string prefix, int lenght)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException(nameof(prefix));
            }

            if (lenght <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lenght), $"Parameter {nameof(lenght)} must by more than zero.");
            }

            string name = string.Concat(prefix, Guid.NewGuid().ToString("D").Replace("-", string.Empty));

            return name;
        }

        public string CreateTypeName()
        {
            string name = string.Concat("T", Guid.NewGuid().ToString("D").Replace("-", string.Empty));

            return name;
        }

        public string CreateTypeName(string prefix, int lenght)
        {
            if (prefix == null)
            {
                throw new ArgumentNullException(nameof(prefix));
            }

            if (lenght <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lenght), $"Parameter {nameof(lenght)} must by more than zero.");
            }

            string name = string.Concat(prefix, Guid.NewGuid().ToString("D").Replace("-", string.Empty));

            return name;
        }
    }
}
