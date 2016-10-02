using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Tests.TestInterfaces
{
    public interface ICompositeInterface : IGrapth, IDisposable, IFormattable
    {
        void AddChild(ICompositeInterface item);

        void Show(double x, double y, double width, double height);
    }
}
