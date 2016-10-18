using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.ValueProviders;
using System.Web.Http.ValueProviders.Providers;

namespace MessageLogger.Services.Infrastructure
{
    public class HeaderValueProviderFactory : ValueProviderFactory
    {
        public override IValueProvider GetValueProvider(HttpActionContext actionContext)
        {
            Dictionary<string, string> headerValuesDictionary = new Dictionary<string, string>();

            foreach (KeyValuePair<string, IEnumerable<string>> header in actionContext.ControllerContext.Request.Headers)
                headerValuesDictionary.Add(header.Key, header.Value.First());

            return new NameValuePairsValueProvider(headerValuesDictionary, CultureInfo.InvariantCulture);
        }
    }
}