using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WcfForHipsters.WebServer.Contract;
using WcfForHipsters.WebServer.WcfForHipsters;

namespace WcfForHipsters.WebServer.Controllers
{
    [Route("api/[controller]")]
    public class ExampleServiceController : Controller
    {
        private readonly ILogger<ExampleServiceController> logger;
        private readonly EndpointAdapter<IExampleService> serviceAdapter;

        public ExampleServiceController(EndpointAdapter<IExampleService> serviceAdapter, ILogger<ExampleServiceController> logger)
        {
            if (serviceAdapter == null)
            {
                throw new ArgumentNullException(nameof(serviceAdapter));
            }
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.serviceAdapter = serviceAdapter;
            this.logger = logger;
        }

        // api/ExampleService POST
        [HttpPost]
        public IActionResult Post([FromBody]JObject value)
        {
            this.logger.LogTrace("Endpoint invoked");
            try
            {
                ResponseBody response = this.serviceAdapter.ProcessCall(value);
                return this.Json(response);
            }
            catch (FaultException ex)
            {
                this.logger.LogError("Exception during call Rpc method {0}", ex.InnerException);
                return this.Json(ex.FaultBody);
            }
        }
    }
}
