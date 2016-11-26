using MassiveDynamicProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApplication.Services.Interceptors
{
    public class ChangeAutorInterceptor : ICallableInterceptor
    {
        private readonly string name;

        public ChangeAutorInterceptor(string name)
        {
            this.name = name;
        }
        public void Intercept(ICallableInvocation invocation)
        {
            invocation.Process();

            Article article = invocation.ReturnValue as Article;
            if (article != null)
            {
                article.Autor = this.name;
            }
        }
    }
}
