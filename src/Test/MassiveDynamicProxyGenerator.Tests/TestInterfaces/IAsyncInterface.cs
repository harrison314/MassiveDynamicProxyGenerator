using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Tests.TestInterfaces
{
    public interface IAsyncInterface
    {
        Task WriteContextAsync(IGrapth graph);

        void WriteContext(IGrapth graph);

        Task<int> GetCountAsync();

        int GetCount();

        IGrapth ReadContext();

        Task<IGrapth> ReadContextAsync();
    }
}
