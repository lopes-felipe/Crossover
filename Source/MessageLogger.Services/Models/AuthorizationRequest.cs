using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MessageLogger.Services.Models
{
    [DataContract]
    public class AuthorizationRequest
    {
        [DataMember(Name = "access_token")]
        public Guid AccessToken { get; set; }
    }
}