using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator.Tests.TestInterfaces
{
    public struct MyStruct
    {
        public int Address;
        public long UsLong;
        public char Asci;
        public string PointAbc;
        public IntPtr Pointer;

        public static MyStruct InitializeDefault()
        {
            MyStruct s = new MyStruct();
            s.Address = 158;
            s.Asci = 'h';
            s.PointAbc = "male janiatko";
            s.Pointer = IntPtr.Zero;
            s.UsLong = 458562544L;

            return s;
        }
    }

    public interface IReturnTypes
    {
        StringBuilder CreateSb(string arg);

        int GetLength(string arg);

        void GetVoid();

        MyStruct GetStruct();

        int PulseEx(int a, string message, Exception ex);
    }
}
