using MessageLogger.Business;
using MessageLogger.Exceptions;
using MessageLogger.Services.Infrastructure;
using MessageLogger.Services.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;

namespace MessageLogger.Services.Controllers
{
    public class DefaultController : ApiController
    {
        public DefaultController()
        {
            this.logLevelSwitch = new TraceSwitch("MessageLogger.Services", "MessageLogger::Services - Default");
        }

        private TraceSwitch logLevelSwitch = null;

        [HttpPost]
        public AuthenticateResponse Auth([ModelBinder]AuthenticateRequest request)
        {
            try
            {
                //----------------------------------------------------------------------------------------------------------------------------------
                // Validate request
                if (string.IsNullOrEmpty(request.Authorization))
                    throw new HttpResponseException(HttpStatusCode.BadRequest);

                if (request.Authorization.StartsWith("Basic "))
                    request.Authorization = request.Authorization.Substring(6);

                //----------------------------------------------------------------------------------------------------------------------------------
                // Authenticate
                using (SqlConnection sqlConnection = GetSqlConnection())
                {
                    Guid accessToken = Authenticate(request, sqlConnection);
                    return new AuthenticateResponse(accessToken);
                }
            }
            catch (ForbiddenException ex)
            {
                Trace.WriteLineIf(this.logLevelSwitch.TraceInfo, string.Format("DafaultController::Auth - {0}.", ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, HttpErrorBuilder.Create(ex)));
            }
            catch (UnauthorizedException ex)
            {
                Trace.WriteLineIf(this.logLevelSwitch.TraceInfo, string.Format("DafaultController::Auth - {0}.", ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, HttpErrorBuilder.Create(ex)));
            }
            catch (HttpResponseException ex)
            {
                Trace.WriteLineIf(this.logLevelSwitch.TraceInfo, string.Format("DafaultController::Auth - {0}.", ex));
                throw;
            }
            catch (InternalException ex)
            {
                Trace.WriteLineIf(this.logLevelSwitch.TraceError, string.Format("DafaultController::Auth - {0}.", ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, HttpErrorBuilder.Create(ex)));
            }
            catch (Exception ex)
            {
                Trace.WriteLineIf(this.logLevelSwitch.TraceError, string.Format("DafaultController::Auth - {0}.", ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, HttpErrorBuilder.Create(new UnexpectedException(90001, Tracing.FormatException(ex)))));
            }
        }

        private Guid Authenticate(AuthenticateRequest request, SqlConnection sqlConnection)
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            // Authentication
            SessionManager sessionManager = GetSessionManager(sqlConnection);

            // Credentials are encoded
            if (!request.Authorization.Contains(":"))
                return sessionManager.Authenticate(request.Authorization);

            // Credentials are in plain text
            string[] authorizationParts = request.Authorization.Split(':');

            if (authorizationParts.Length != 2)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            string applicationID = authorizationParts[0];
            string secret = authorizationParts[1];

            //----------------------------------------------------------------------------------------------------------------------------------
            // Call Registration
            // TODO: Move this logic to an ActionFilter
            ApplicationManager applicationManager = GetApplicationManager(sqlConnection);
            applicationManager.RegisterCall(applicationID);

            return sessionManager.Authenticate(applicationID, secret);
        }

        [HttpPost]
        public RegisterResponse Register(RegisterRequest request)
        {
            try
            {
                //----------------------------------------------------------------------------------------------------------------------------------
                // Validate request
                if (string.IsNullOrEmpty(request.DisplayName))
                    throw new HttpResponseException(HttpStatusCode.BadRequest);

                //----------------------------------------------------------------------------------------------------------------------------------
                // Register
                using (SqlConnection sqlConnection = GetSqlConnection())
                {
                    ApplicationManager applicationManager = new ApplicationManager(new Data.ApplicationDataAccess(sqlConnection), new Data.ApplicationCallDataAccess(sqlConnection));
                    Entities.Application application = applicationManager.CreateApplication(request.DisplayName);

                    return new RegisterResponse(application.ApplicationID, application.Secret, application.DisplayName);
                }
            }
            catch (ForbiddenException ex)
            {
                Trace.WriteLineIf(this.logLevelSwitch.TraceInfo, string.Format("DafaultController::Register - {0}.", ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, HttpErrorBuilder.Create(ex)));
            }
            catch (UnauthorizedException ex)
            {
                Trace.WriteLineIf(this.logLevelSwitch.TraceInfo, string.Format("DafaultController::Register - {0}.", ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, HttpErrorBuilder.Create(ex)));
            }
            catch (HttpResponseException ex)
            {
                Trace.WriteLineIf(this.logLevelSwitch.TraceInfo, string.Format("DafaultController::Register - {0}.", ex));
                throw;
            }
            catch (InternalException ex)
            {
                Trace.WriteLineIf(this.logLevelSwitch.TraceError, string.Format("DafaultController::Register - {0}.", ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, HttpErrorBuilder.Create(ex)));
            }
            catch (Exception ex)
            {
                Trace.WriteLineIf(this.logLevelSwitch.TraceError, string.Format("DafaultController::Register - {0}.", ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, HttpErrorBuilder.Create(new UnexpectedException(90001, Tracing.FormatException(ex)))));
            }
        }

        [HttpPost]
        public LogResponse Log([ModelBinder]AuthenticateRequest authentication, LogRequest request)
        {
            try
            {
                //----------------------------------------------------------------------------------------------------------------------------------
                // Validate request
                if (string.IsNullOrEmpty(request.ApplicationID) || string.IsNullOrEmpty(request.Level) || string.IsNullOrEmpty(request.Logger) || string.IsNullOrEmpty(request.Message))
                    throw new HttpResponseException(HttpStatusCode.BadRequest);

                using (SqlConnection sqlConnection = GetSqlConnection())
                {
                    //----------------------------------------------------------------------------------------------------------------------------------
                    // Token authorization
                    // TODO: Move this logic to an AuthorizationFilter
                    Guid accessToken = Guid.Empty;

                    if (!Guid.TryParse(authentication.Authorization, out accessToken))
                        throw new HttpResponseException(HttpStatusCode.BadRequest);

                    SessionManager sessionManager = GetSessionManager(sqlConnection);
                    sessionManager.Authorize(accessToken);

                    //----------------------------------------------------------------------------------------------------------------------------------
                    // Call Registration
                    // TODO: Move this logic to an ActionFilter
                    ApplicationManager applicationManager = GetApplicationManager(sqlConnection);
                    applicationManager.RegisterCall(request.ApplicationID);

                    //----------------------------------------------------------------------------------------------------------------------------------
                    // Log
                    Logger logger = new Logger(new Data.LogDataAccess(sqlConnection));
                    bool success = logger.Log(request.ApplicationID, request.Logger, request.Level, request.Message);

                    return new LogResponse(success);
                }
            }
            catch (ForbiddenException ex)
            {
                Trace.WriteLineIf(this.logLevelSwitch.TraceInfo, string.Format("DafaultController::Log - {0}.", ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Forbidden, HttpErrorBuilder.Create(ex)));
            }
            catch (UnauthorizedException ex)
            {
                Trace.WriteLineIf(this.logLevelSwitch.TraceInfo, string.Format("DafaultController::Log - {0}.", ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.Unauthorized, HttpErrorBuilder.Create(ex)));
            }
            catch (HttpResponseException ex)
            {
                Trace.WriteLineIf(this.logLevelSwitch.TraceInfo, string.Format("DafaultController::Log - {0}.", ex));
                throw;
            }
            catch (InternalException ex)
            {
                Trace.WriteLineIf(this.logLevelSwitch.TraceError, string.Format("DafaultController::Log - {0}.", ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, HttpErrorBuilder.Create(ex)));
            }
            catch (Exception ex)
            {
                Trace.WriteLineIf(this.logLevelSwitch.TraceError, string.Format("DafaultController::Log - {0}.", ex));
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, HttpErrorBuilder.Create(new UnexpectedException(90001, Tracing.FormatException(ex)))));
            }
        }

        private SqlConnection GetSqlConnection()
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Crossover"].ConnectionString);
            connection.Open();

            return connection;
        }

        private SessionManager GetSessionManager(SqlConnection sqlConnection)
        {
            // TODO: Change it for a DI container
            return new SessionManager(new Data.ApplicationDataAccess(sqlConnection), new Data.ApplicationSessionDataAccess(sqlConnection), new Data.SessionConfigurationDataAccess(sqlConnection));
        }

        private ApplicationManager GetApplicationManager(SqlConnection sqlConnection)
        {
            // TODO: Change it for a DI container
            return new ApplicationManager(new Data.ApplicationDataAccess(sqlConnection), new Data.ApplicationCallDataAccess(sqlConnection));
        }
    }
}
