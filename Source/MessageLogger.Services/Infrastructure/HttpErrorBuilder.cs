using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace MessageLogger.Services.Infrastructure
{
    public class HttpErrorBuilder
    {
        public static HttpError Create(Exceptions.BaseException exception)
        {
            return Create(exception, false);
        }

        public static HttpError Create(Exceptions.BaseException exception, bool includeStackTrace)
        {
            HttpError error = new HttpError();
            error.Add("ErrorCode", exception.ErrorCode);
            error.Add("Message", exception.Message);

            if (includeStackTrace)
                error.Add("StackTrace", exception.StackTrace);

            return error;
        }
    }
}