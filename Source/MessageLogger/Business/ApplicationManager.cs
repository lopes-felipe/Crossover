using MessageLogger.Data;
using MessageLogger.Entities;
using MessageLogger.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MessageLogger.Business
{
    public class ApplicationManager
    {
        public ApplicationManager(DataAccess<Application> applicationDataAccess, DataAccess<ApplicationCall> applicationCallDataAccess)
        {
            this.applicationDataAccess = applicationDataAccess;
            this.applicationCallDataAccess = applicationCallDataAccess;
        }

        private DataAccess<Application> applicationDataAccess = null;
        private DataAccess<ApplicationCall> applicationCallDataAccess = null;

        /// <summary>
        /// Creates a new Application
        /// </summary>
        /// <param name="displayName">Application's name</param>
        /// <returns>Application</returns>
        public Application CreateApplication(string displayName)
        {
            // Generates ApplicationID e ApplicationSecret randomly
            string applicationID = GetRandomString(32);
            string applicationSecret = GetRandomString(25);

            // Creates the new Application
            Application newAplication = new Application(applicationID, displayName, applicationSecret);
            this.applicationDataAccess.Create(newAplication);

            return newAplication;
        }

        /// <summary>
        /// Creates a new call entry
        /// </summary>
        /// <param name="applicationID">ID of the application responsible for the call</param>
        public void RegisterCall(string applicationID)
        {
            // Get current application to check if it has restricted access
            Application application = new Application(applicationID);
            application = this.applicationDataAccess.Retrieve(application).FirstOrDefault();

            if (application == null)
                throw new AuthenticationException(10003, "Invalid Application ID");

            // Register the new call
            ApplicationCall newApplicationCall = new ApplicationCall(0, applicationID, DateTime.Now);
            newApplicationCall = this.applicationCallDataAccess.Create(newApplicationCall);
            
            // If it does, the restriction is reseted
            if (application.RestrictedAccessUntil > DateTime.Now)
            {
                application.RestrictedAccessUntil = DateTime.Now.AddMinutes(5);
                this.applicationDataAccess.Update(application);

                throw new Exceptions.RateLimitExceededException(20001, "Rate limit exceeded");
            }

            // Get all the calls of this application, in the last minute
            ApplicationCall currentApplicationCall = new ApplicationCall(0, applicationID, DateTime.MinValue);
            IEnumerable<ApplicationCall> currentApplicationCalls = ((ApplicationCallDataAccess)this.applicationCallDataAccess).Retrieve(currentApplicationCall, DateTime.Now.AddMinutes(-1), DateTime.Now); //TODO: Refactor to an interface

            // If the nunber of calls exceds the configured value, then an access restriction is set
            int rateLimit = int.Parse(ConfigurationManager.AppSettings["ApplicationCalls.RateLimit"]);

            if (currentApplicationCalls.Count() >= rateLimit)  
            {
                application.RestrictedAccessUntil = DateTime.Now.AddMinutes(5);
                this.applicationDataAccess.Update(application);

                throw new Exceptions.RateLimitExceededException(20002, "Rate limit exceeded");
            }
        }

        private string GetRandomString(int length)
        {
            string possibleChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[length];

            // Perhaps change it for a more secure implementation, such as RNGCryptoServiceProvider
            Random random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
                stringChars[i] = possibleChars[random.Next(possibleChars.Length)];

            return new String(stringChars);
        }
    }
}
