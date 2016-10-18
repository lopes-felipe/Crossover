using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MessageLogger.Services.Models
{
    [DataContract]
    public class LogResponse
    {
        public LogResponse()
        {

        }

        public LogResponse(bool success)
            : this()
        {
            this.Success = success;
        }

        [DataMember(Name = "success")]
        public bool Success { get; set; }
    }
}