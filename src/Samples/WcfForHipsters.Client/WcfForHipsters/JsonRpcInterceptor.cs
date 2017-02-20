using MassiveDynamicProxyGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WcfForHipsters.Client.WcfForHipsters
{
    internal class JsonRpcInterceptor : IInterceptor
    {
        private readonly Uri endpoint;

        public JsonRpcInterceptor(Uri endpoint)
        {
            if (endpoint == null) throw new ArgumentNullException(nameof(endpoint));

            this.endpoint = endpoint;
        }

        public void Intercept(IInvocation invocation, bool isDynamicInterception)
        {
            Guid requestId = Guid.NewGuid();
            var rcpBody = new
            {
                jsonrpc = "2.0",
                method = invocation.MethodName,
                @params = invocation.Arguments,
                id = requestId.ToString()
            };

            string serialized = Newtonsoft.Json.JsonConvert.SerializeObject(rcpBody, Newtonsoft.Json.Formatting.Indented);
            ServiseResponse response = this.CallServise(serialized).GetAwaiter().GetResult(); //TODO: to asnc interceptor

            if (response.StatusCode == 200)
            {
                Newtonsoft.Json.Linq.JToken errorObject;
                Newtonsoft.Json.Linq.JObject rpcResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(response.Content);
                if (rpcResponse.TryGetValue("error", out errorObject))
                {
                    string fault = errorObject.ToObject<string>();
                    throw new RpcFaultException(fault);
                }

                if (invocation.ReturnType != typeof(void))
                {
                    invocation.ReturnValue = rpcResponse["result"].ToObject(invocation.ReturnType);
                }
            }
            else
            {
                throw new RpcFaultException($"Service {invocation.MethodName} from {this.endpoint} return {response.StatusCode}");
            }
        }

        private async Task<ServiseResponse> CallServise(string serialized)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                StringContent content = new StringContent(serialized, Encoding.UTF8, "application/json");
                using (HttpResponseMessage response = await client.PostAsync(this.endpoint, content))
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    return new ServiseResponse((int)response.StatusCode, responseContent);
                }
            }
        }
    }
}
