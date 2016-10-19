using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProxyGeneratrorSamples.Net40
{
    public class Calculator : ICalculator
    {
        public Calculator()
        {

        }

        public int Add(int a, int b)
        {
            return a + b;
        }

        public int Modulo(int a, int b)
        {
            if (b < 1)
            {
                throw new ArgumentOutOfRangeException("Parameter b can not by less of one.");
            }

            return a % b;
        }

        public int Product(int a, int b)
        {
            return a * b;
        }
    }
}
