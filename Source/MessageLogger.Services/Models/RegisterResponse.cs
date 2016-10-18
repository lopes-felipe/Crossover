using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MessageLogger.Services.Models
{
    [DataContract]
    public class RegisterResponse
    {
        public RegisterResponse()
        {

        }

        public RegisterResponse(string applicationID, string applicationSecret, string displayName)
            : this()
        {
            this.ApplicationID = applicationID;
            this.ApplicationSecret = applicationSecret;
            this.DisplayName = displayName;
        }

        [DataMember(Name = "application_id")]
        public string ApplicationID { get; set; }

        [DataMember(Name = "application_secret")]
        public string ApplicationSecret { get; set; }

        [DataMember(Name = "display_name")]
        public string DisplayName { get; set; }
    }
}