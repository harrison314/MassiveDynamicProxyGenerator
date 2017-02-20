using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WcfForHipsters.WebServer.Contract;

namespace WcfForHipsters.WebServer.Services
{
    public class ExampleServise : IExampleServise
    {
        private ILogger<ExampleServise> logger;

        public ExampleServise(ILogger<ExampleServise> logger)
        {
            this.logger = logger;
        }

        public int CalCulateAdd(int a, int b, int c)
        {
            this.logger.LogTrace("CalCulateAdd: a={0}, b={1}, c={2}", a, b, c);

            if (c < -150)
            {
                this.logger.LogError("Parameter c is out of range.");
                throw new ArgumentOutOfRangeException($"Parameter {nameof(c)} is out of range. Must by more as -150.");
            }

            return a + b + c;
        }

        public CreatBookResponse CreateBook(CreateBookRequest request)
        {
            this.logger.LogTrace("CreateBook with title: {0}", request.Title);

            CreatBookResponse response = new CreatBookResponse();
            response.CreateionTime = DateTime.Now;
            response.Id = Guid.NewGuid();
            response.PublicUrl = "https://temuri.org/" + response.Id.ToString();
            response.UserId = request.MarkdawnText.Length;

            return response;
        }

        public void SendEvent(Guid id, string content, RequestMetadata metadata)
        {
            this.logger.LogTrace("SendEvent: id={0}, content={1}, c={2}", id, content);
        }
    }
}
