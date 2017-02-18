using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;

namespace WcfForHipsters.WebServer.WcfForHipsters
{
    public class EndpointAdapter<T> where T : class
    {
        private T implementation;
        private IReadOnlyDictionary<string, Func<T, JToken[], object>> methodDelegates;

        public EndpointAdapter(T implementation)
        {
            if (implementation == null) throw new ArgumentNullException(nameof(implementation));

            this.implementation = implementation;

            try
            {
                this.methodDelegates = this.CreateDelegates();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw;
            }
        }
        public ResponseBody ProcessCall(JToken request)
        {
            string id = "undefindet";
            try
            {
                string method = request["method"].ToObject<string>();
                id = request["id"].ToObject<string>();
                JToken[] arrayParams = request["params"].ToArray<JToken>();

                return this.ProcessCall(method, id, arrayParams);
            }
            catch (Exception ex)
            {
                throw new FaultException("Pocess call fault see inner exception.", id, ex);
            }
        }

        private ResponseBody ProcessCall(string method, string id, JToken[] arrayParams)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));

            Func<T, JToken[], object> delegat;
            if (this.methodDelegates.TryGetValue(method, out delegat))
            {
                ResponseBody response = new ResponseBody();
                response.jsonrpc = "2.0";
                response.result = delegat(this.implementation, arrayParams);
                response.id = id;
                return response;
            }
            else
            {
                throw new InvalidOperationException($"Mehod {method} not found.");
            }
        }

        private Dictionary<string, Func<T, JToken[], object>> CreateDelegates()
        {
            Dictionary<string, Func<T, JToken[], object>> delegates = new Dictionary<string, Func<T, JToken[], object>>();
            MethodInfo[] methods = typeof(T).GetTypeInfo().GetMethods();

            foreach (MethodInfo methodInfo in methods)
            {
                delegates.Add(methodInfo.Name, this.EmitDelegate(methodInfo));
            }

            return delegates;
        }

        private Func<T, JToken[], object> EmitDelegate(MethodInfo methodInfo)
        {
            ParameterExpression instanceParameter = Expression.Parameter(typeof(T), "instance");
            ParameterExpression paramsParameter = Expression.Parameter(typeof(JToken[]), "parameters");

            MethodInfo toObjectMethod = typeof(JToken).GetTypeInfo().GetMethod(nameof(JToken.ToObject), new Type[] { typeof(Type) });

            ParameterInfo[] parameters = methodInfo.GetParameters();
            Expression[] parametersExpression = new Expression[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                Expression arrayAcess = Expression.ArrayAccess(paramsParameter, Expression.Constant(i, typeof(int)));
                Expression callDeserialize = Expression.Call(arrayAcess, toObjectMethod, Expression.Constant(parameters[i].ParameterType, typeof(Type)));
                parametersExpression[i] = Expression.Convert(callDeserialize, parameters[i].ParameterType);
            }

            MethodCallExpression callExpresion = Expression.Call(instanceParameter, methodInfo, arguments: parametersExpression);

            if (methodInfo.ReturnType == typeof(void))
            {
                BlockExpression block = Expression.Block(callExpresion, Expression.Constant(null, typeof(object)));

                return Expression.Lambda<Func<T, JToken[], object>>(block, instanceParameter, paramsParameter)
                    .Compile();
            }
            else
            {
                UnaryExpression boxedReturn = Expression.Convert(callExpresion, typeof(object));
                return Expression.Lambda<Func<T, JToken[], object>>(boxedReturn, instanceParameter, paramsParameter)
                    .Compile();
            }
        }
    }
}
