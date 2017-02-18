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
    public class ExampleServiseController : Controller
    {
        private readonly ILogger<ExampleServiseController> logger;
        private readonly EndpointAdapter<IExampleServise> serviceAdapter;

        public ExampleServiseController(EndpointAdapter<IExampleServise> serviceAdapter, ILogger<ExampleServiseController> logger)
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

        // api/ExampleServise POST
        [HttpPost]
        public IActionResult Post([FromBody]JObject value)
        {
            this.logger.LogTrace("Enpoint invoked");
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
