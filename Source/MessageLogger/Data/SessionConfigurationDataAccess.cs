using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageLogger.Entities;

namespace MessageLogger.Data
{
    public class SessionConfigurationDataAccess
        : DataAccess<SessionConfiguration>
    {
        public SessionConfigurationDataAccess(SqlConnection connection)
        {
            this.connection = connection;
        }

        private SqlConnection connection = null;

        internal override Entities.SessionConfiguration Create(SessionConfiguration entity)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = this.connection;
                command.CommandText = @"INSERT INTO [session_configuration]
                                                   ([configuration_id], [session_lifetime_minutes])
                                            VALUES (@configuration_id, @session_lifetime_minutes) ";

                command.Parameters.Add(CreateParameter(command, "configuration_id", entity.ConfigurationID));
                command.Parameters.Add(CreateParameter(command, "session_lifetime_minutes", entity.SessionLifetimeMinutes));

                command.ExecuteNonQuery();

                return entity;
            }
        }

        internal override int Update(SessionConfiguration entity)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = this.connection;
                command.CommandText = @"UPDATE [session_configuration] SET 
                                               [session_lifetime_minutes] = @session_lifetime_minutes 
                                         WHERE [configuration_id] = @configuration_id ";

                command.Parameters.Add(CreateParameter(command, "configuration_id", entity.ConfigurationID));
                command.Parameters.Add(CreateParameter(command, "session_lifetime_minutes", entity.SessionLifetimeMinutes));

                return command.ExecuteNonQuery();
            }
        }

        internal override int Delete(SessionConfiguration entity)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = this.connection;
                command.CommandText = @"DELETE [session_configuration]
                                         WHERE [configuration_id] = @configuration_id ";

                command.Parameters.Add(CreateParameter(command, "configuration_id", entity.ConfigurationID));
                return command.ExecuteNonQuery();
            }
        }

        internal override IEnumerable<SessionConfiguration> Retrieve(SessionConfiguration entity)
        {
            List<SessionConfiguration> sessionConfigurationList = new List<SessionConfiguration>();

            using (SqlCommand command = new SqlCommand())
            {
                //------------------------------------------------------------------------------------------------------
                // Command Build
                command.Connection = this.connection;
                command.CommandText = @"SELECT [configuration_id], [session_lifetime_minutes]
                                          FROM [session_configuration] ";

                if (entity != null && entity.ConfigurationID > 0)
                {
                    command.CommandText += (command.Parameters.Count == 0 ? "WHERE " : "AND ") + "[configuration_id] = @configuration_id ";
                    command.Parameters.Add(CreateParameter(command, "configuration_id", entity.ConfigurationID));
                }

                if (entity != null && entity.SessionLifetimeMinutes > 0)
                {
                    command.CommandText += (command.Parameters.Count == 0 ? "WHERE " : "AND ") + "[session_lifetime_minutes] = @session_lifetime_minutes ";
                    command.Parameters.Add(CreateParameter(command, "session_lifetime_minutes", entity.SessionLifetimeMinutes));
                }

                //------------------------------------------------------------------------------------------------------
                // Reader
                SqlDataReader reader = command.ExecuteReader();

                bool read = reader.Read();

                while (read)
                {
                    sessionConfigurationList.Add(new SessionConfiguration(
                        (int)reader["configuration_id"],
                        (int)reader["session_lifetime_minutes"]));

                    read = reader.Read();
                }

                reader.Close();
            }

            return sessionConfigurationList;
        }
    }
}
