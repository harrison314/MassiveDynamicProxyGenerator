using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WcfForHipsters.WebServer.Contract
{
    public class CreatBookResponse
    {
        public Guid Id
        {
            get;
            set;
        }

        public int UserId
        {
            get;
            set;
        }

        public DateTime CreateionTime
        {
            get;
            set;
        }

        public string PublicUrl
        {
            get;
            set;
        }

        public CreatBookResponse()
        {

        }
    }
}
