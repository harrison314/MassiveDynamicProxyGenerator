using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyGeneratrorSamples.Net40
{
    public interface ICalculator
    {
        int Add(int a, int b);

        int Product(int a, int b);

        int Modulo(int a, int b);
    }
}
