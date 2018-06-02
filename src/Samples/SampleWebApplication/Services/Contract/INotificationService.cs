using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleWebApplication.Services.Contract
{
    public interface INotificationService
    {
        void NotifyRead(string itemName, object id);

        void NotifyUpdate(string itemName, object id);

        void NotifyInsert(string itemName, object id);

        void NotifyDelete(string itemName, object id);
    }
}
