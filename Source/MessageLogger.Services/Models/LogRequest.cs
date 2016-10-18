using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MessageLogger.Services.Models
{
    [DataContract]
    public class LogRequest
    {
        [DataMember(Name = "application_id")]
        public string ApplicationID { get; set; }

        [DataMember(Name = "logger")]
        public string Logger { get; set; }

        [DataMember(Name = "level")]
        public string Level { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}