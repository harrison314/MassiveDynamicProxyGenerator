using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WcfForHipsters.Client.WcfForHipsters
{
    internal struct ServiseResponse
    {
        public readonly int StatusCode;

        public readonly string Content;

        public ServiseResponse(int statusCode, string responseContent) : this()
        {
            this.StatusCode = statusCode;
            this.Content = responseContent;
        }
    }
}
