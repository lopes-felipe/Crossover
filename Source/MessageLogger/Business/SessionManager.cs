using MessageLogger.Data;
using MessageLogger.Entities;
using MessageLogger.Exceptions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLogger.Business
{
    public class SessionManager
    {
        public SessionManager(DataAccess<Application> applicationDataAccess, DataAccess<ApplicationSession> sessionDataAccess, DataAccess<SessionConfiguration> sessionConfigurationDataAccess)
        {
            this.applicationDataAccess = applicationDataAccess;
            this.sessionDataAccess = sessionDataAccess;
            this.sessionConfigurationDataAccess = sessionConfigurationDataAccess;

            this.logLevelSwitch = new TraceSwitch("MessageLogger.SessionManager", "MessageLogger::SessionManager - Switch");
        }

        private DataAccess<Application> applicationDataAccess = null;
        private DataAccess<ApplicationSession> sessionDataAccess = null;
        private DataAccess<SessionConfiguration> sessionConfigurationDataAccess = null;

        private TraceSwitch logLevelSwitch = null;

        /// <summary>
        /// Validates the provided credentials and generates (if necessary) a new access token
        /// </summary>
        /// <param name="encodedCredentials">The authentication credentials encoded in RFC2045-MIME (Base64)</param>
        /// <returns>Access Token</returns>
        public Guid Authenticate(string encodedCredentials)
        {
            //----------------------------------------------------------------------------------------------------------------
            // Credentials extraction
            string decodedCredentials = Base64Encoder.DecodeString(encodedCredentials);
            string[] authorizationParts = decodedCredentials.Split(':');

            if (authorizationParts.Length != 2)
                throw new AuthenticationException(10002, "Invalid credentials");

            string applicationID = authorizationParts[0];
            string secret = authorizationParts[1];

            //----------------------------------------------------------------------------------------------------------------
            // Authentication
            return Authenticate(applicationID, secret);
        }

        /// <summary>
        /// Validates the provided credentials and generates (if necessary) a new access token
        /// </summary>
        /// <param name="applicationID">Application ID</param>
        /// <param name="secret">Secret</param>
        /// <returns>Access Token</returns>
        public Guid Authenticate(string applicationID, string secret)
        {
            Trace.WriteLineIf(this.logLevelSwitch.TraceVerbose, "SessionManager::Authenticate - Starting authentication.");
            
            //----------------------------------------------------------------------------------------------------------------
            // Application authentication
            Application application = new Application(applicationID, string.Empty, secret);
            IEnumerable<Application> applications = applicationDataAccess.Retrieve(application);

            if (applications.Count() == 0)
                throw new AuthenticationException(10001, "Invalid credentials");
            
            //----------------------------------------------------------------------------------------------------------------
            // Session handling
            ApplicationSession session = CreateSession(applicationID);

            Trace.WriteLineIf(this.logLevelSwitch.TraceVerbose, "SessionManager::Authenticate - Authentication successfully finished.");

            return session.AccessToken;
        }
        

        /// <summary>
        /// Validate session permissions
        /// </summary>
        /// <param name="accessToken">Session authentication token</param>
        public void Authorize(Guid accessToken)
        {
            //----------------------------------------------------------------------------------------------------------------
            // Retrieve Session
            ApplicationSession session = new ApplicationSession() { AccessToken = accessToken };
            IEnumerable<ApplicationSession> sessions = this.sessionDataAccess.Retrieve(session);

            // Provided token is invalid
            if (sessions.Count() == 0)
                throw new InvalidTokenException(30001, string.Format("Token '{0}' not found.", accessToken)); 

            //----------------------------------------------------------------------------------------------------------------
            // Session validation
            session = sessions.First();

            // Session is still valid
            if (session.ValidUntil > DateTime.Now)
                return;

            // Session is no longer valid, so it must be updated
            if (session.Active == true)
            {
                session.Active = false;
                this.sessionDataAccess.Update(session);
            }

            throw new InvalidTokenException(30002, string.Format("Token '{0}' has expired.", accessToken));
        }

        private ApplicationSession CreateSession(string applicationID)
        {
            //----------------------------------------------------------------------------------------------------------------
            // Retrieve Session
            ApplicationSession session = new ApplicationSession(0, applicationID, Guid.Empty, true, DateTime.MinValue, DateTime.MinValue);

            // Search for currently active session for this ApplicationID
            IEnumerable<ApplicationSession> sessions = sessionDataAccess.Retrieve(session);

            // There is an active session
            if (sessions.Count() > 0)
            {
                ApplicationSession currentSession = sessions.First();

                // Current Session is still valid. It will be returned
                if (currentSession.ValidUntil > DateTime.Now)
                    return currentSession;

                // Current Session is no longer valid. It is updated to not active and a new one will be created
                currentSession.Active = false;
                sessionDataAccess.Update(currentSession);
            }

            //----------------------------------------------------------------------------------------------------------------
            // Create Session
            SessionConfiguration sessionConfiguration = GetSessionConfiguration();
            session.AccessToken = Guid.NewGuid();
            session.CreatedDate = DateTime.Now;
            session.ValidUntil = DateTime.Now.AddMinutes(sessionConfiguration.SessionLifetimeMinutes);

            session = sessionDataAccess.Create(session);
            return session;
        }

        private SessionConfiguration GetSessionConfiguration()
        {
            // TODO: Implement cache for the Session Configuration
            IEnumerable<SessionConfiguration> sessionConfigurations = sessionConfigurationDataAccess.Retrieve();

            if (sessionConfigurations.Count() == 0)
                throw new BackendInfrastructureException(40001, "Missing initial Session Configuration entry"); 

            SessionConfiguration sessionConfiguration = sessionConfigurations.First();
            return sessionConfiguration;
        }
    }
}
