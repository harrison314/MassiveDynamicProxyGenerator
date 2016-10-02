﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MassiveDynamicProxyGenerator
{
    public interface ICallableInterceptor
    {
        void Intercept(ICallableInvocation invocation);
    }
}
