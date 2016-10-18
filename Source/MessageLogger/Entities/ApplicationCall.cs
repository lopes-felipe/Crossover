using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLogger.Entities
{
    public class ApplicationCall
    {
        public ApplicationCall()
        {

        }

        public ApplicationCall(long callID)
            : this()
        {
            this.CallID = callID;
        }

        public ApplicationCall(long callID, string applicationID, DateTime callDate)
            : this(callID)
        {
            this.ApplicationID = applicationID;
            this.CallDate = callDate;
        }

        public long CallID { get; set; }

        public string ApplicationID { get; set; }

        public DateTime CallDate { get; set; }
    }
}
