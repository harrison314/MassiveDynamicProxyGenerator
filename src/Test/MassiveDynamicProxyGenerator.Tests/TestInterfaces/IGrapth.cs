using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Tests.TestInterfaces
{
    public interface IGrapth
    {
        Guid Id
        {
            get;
        }

        string Name
        {
            get;
        }

        string DisplayName
        {
            get;
            set;
        }

        double[,] CacluateTable(DateTime from, DateTime to);

        IGrapth Combine(IGrapth other);
    }
}
