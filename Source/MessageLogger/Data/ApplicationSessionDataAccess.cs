using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageLogger.Entities;

namespace MessageLogger.Data
{
    public class ApplicationSessionDataAccess
        : DataAccess<ApplicationSession>
    {
        public ApplicationSessionDataAccess(SqlConnection connection)
        {
            this.connection = connection;
        }

        private SqlConnection connection = null;

        internal override ApplicationSession Create(ApplicationSession entity)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = this.connection;
                command.CommandText = @"INSERT INTO [application_session]
                                                   ([application_id], [access_token], [active], [created_date], [valid_until])
                                            VALUES (@application_id, @access_token, @active, @created_date, @valid_until) 
                                            SELECT @@IDENTITY ";

                command.Parameters.Add(CreateParameter(command, "application_id", entity.ApplicationID));
                command.Parameters.Add(CreateParameter(command, "access_token", entity.AccessToken));
                command.Parameters.Add(CreateParameter(command, "active", entity.Active));
                command.Parameters.Add(CreateParameter(command, "created_date", entity.CreatedDate));
                command.Parameters.Add(CreateParameter(command, "valid_until", entity.ValidUntil));

                entity.SessionID = command.ExecuteNonQuery();

                return entity;
            }
        }

        internal override int Update(ApplicationSession entity)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = this.connection;
                command.CommandText = @"UPDATE [application_session] SET 
                                               [application_id] = @application_id,
                                               [access_token] = @access_token, 
                                               [active] = @active,
                                               [created_date] = @created_date,
                                               [valid_until] = @valid_until
                                         WHERE [session_id] = @session_id ";

                command.Parameters.Add(CreateParameter(command, "application_id", entity.ApplicationID));
                command.Parameters.Add(CreateParameter(command, "session_id", entity.SessionID));
                command.Parameters.Add(CreateParameter(command, "access_token", entity.AccessToken));
                command.Parameters.Add(CreateParameter(command, "active", entity.Active));
                command.Parameters.Add(CreateParameter(command, "created_date", entity.CreatedDate));
                command.Parameters.Add(CreateParameter(command, "valid_until", entity.ValidUntil));

                return command.ExecuteNonQuery();
            }
        }

        internal override int Delete(ApplicationSession entity)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = this.connection;
                command.CommandText = @"DELETE [application_session]
                                         WHERE [session_id] = @session_id ";

                command.Parameters.Add(CreateParameter(command, "session_id", entity.SessionID));
                return command.ExecuteNonQuery();
            }
        }

        internal override IEnumerable<ApplicationSession> Retrieve(ApplicationSession entity)
        {
            List<ApplicationSession> applicationSessionList = new List<ApplicationSession>();

            using (SqlCommand command = new SqlCommand())
            {
                //------------------------------------------------------------------------------------------------------
                // Command Build
                command.Connection = this.connection;
                command.CommandText = @"SELECT [session_id], [application_id], [access_token], [active], [created_date], [valid_until]
                                          FROM [application_session] ";
                
                if (entity != null && entity.SessionID != 0)
                {
                    command.CommandText += (command.Parameters.Count == 0 ? "WHERE " : "AND ") + "[session_id] = @session_id ";
                    command.Parameters.Add(CreateParameter(command, "session_id", entity.SessionID));
                }

                if (entity != null && !string.IsNullOrEmpty(entity.ApplicationID))
                {
                    command.CommandText += (command.Parameters.Count == 0 ? "WHERE " : "AND ") + "[application_id] = @application_id ";
                    command.Parameters.Add(CreateParameter(command, "application_id", entity.ApplicationID));
                }

                if (entity != null && entity.AccessToken != Guid.Empty)
                {
                    command.CommandText += (command.Parameters.Count == 0 ? "WHERE " : "AND ") + "[access_token] = @access_token ";
                    command.Parameters.Add(CreateParameter(command, "access_token", entity.AccessToken));
                }

                if (entity != null && entity.Active != null)
                {
                    command.CommandText += (command.Parameters.Count == 0 ? "WHERE " : "AND ") + "[active] = @active ";
                    command.Parameters.Add(CreateParameter(command, "active", entity.Active));
                }

                if (entity != null && entity.CreatedDate != DateTime.MinValue)
                {
                    command.CommandText += (command.Parameters.Count == 0 ? "WHERE " : "AND ") + "[created_date] = @created_date ";
                    command.Parameters.Add(CreateParameter(command, "created_date", entity.CreatedDate));
                }

                if (entity != null && entity.ValidUntil != DateTime.MinValue)
                {
                    command.CommandText += (command.Parameters.Count == 0 ? "WHERE " : "AND ") + "[valid_until] = @valid_until ";
                    command.Parameters.Add(CreateParameter(command, "valid_until", entity.ValidUntil));
                }

                //------------------------------------------------------------------------------------------------------
                // Reader
                SqlDataReader reader = command.ExecuteReader();

                bool read = reader.Read();

                while (read)
                {
                    applicationSessionList.Add(new ApplicationSession(
                        (long)reader["session_id"],
                        (string)reader["application_id"],
                        (Guid)reader["access_token"],
                        (bool)reader["active"],
                        (DateTime)reader["created_date"],
                        (DateTime)reader["valid_until"]));

                    read = reader.Read();
                }

                reader.Close();
            }

            return applicationSessionList;
        }
    }
}
