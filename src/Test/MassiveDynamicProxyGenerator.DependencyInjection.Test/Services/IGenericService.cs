using System;
using System.Collections.Generic;
using System.Text;

namespace MassiveDynamicProxyGenerator.DependencyInjection.Test.Services
{
    public interface IGenericService<T>
    {
        T Transform(T item);

        T GetLast();
    }
}
