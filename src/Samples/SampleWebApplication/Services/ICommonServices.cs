using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApplication.Services
{
    public interface ICommonServices
    {
        IHostingEnvironment HostingEnvironment
        {
            get;
        }

        INotificationServise NotificationServise
        {
            get;
        }

        // other srvices
    }
}
