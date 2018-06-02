using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApplication.Services.Contract
{
    public interface ICommonServices
    {
        IHostingEnvironment HostingEnvironment
        {
            get;
        }

        INotificationService NotificationService
        {
            get;
        }

        // other srvices
    }
}
