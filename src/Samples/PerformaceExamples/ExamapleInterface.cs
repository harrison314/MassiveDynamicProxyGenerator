using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformaceExamples
{
    public class ExamapleInterface : IExamapleInterface
    {
        public ExamapleInterface()
        {

        }

        public string Foo(int id, string name, string content)
        {
            return name;
        }
    }
}
