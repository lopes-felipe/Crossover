using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MessageLogger.Services.Models
{
    [DataContract]
    public class RegisterRequest
    {
        [DataMember(Name = "display_name")]
        public string DisplayName { get; set; }
    }
}