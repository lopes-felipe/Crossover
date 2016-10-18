using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageLogger.Entities;

namespace MessageLogger.Data
{
    public class ApplicationDataAccess
        : DataAccess<Application>
    {
        public ApplicationDataAccess(SqlConnection connection)
        {
            this.connection = connection;
        }

        private SqlConnection connection = null;

        internal override Application Create(Application entity)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = this.connection;
                command.CommandText = @"INSERT INTO [application]
                                                   ([application_id], [display_name], [secret], [restricted_access_until])
                                            VALUES (@application_id, @displayName, @secret, @restricted_access_until) ";

                command.Parameters.Add(CreateParameter(command, "application_id", entity.ApplicationID));
                command.Parameters.Add(CreateParameter(command, "displayName", entity.DisplayName));
                command.Parameters.Add(CreateParameter(command, "secret", entity.Secret));
                command.Parameters.Add(CreateParameter(command, "restricted_access_until", entity.RestrictedAccessUntil == DateTime.MinValue ? (object)DBNull.Value : entity.RestrictedAccessUntil));

                command.ExecuteNonQuery();

                return entity;
            }
        }

        internal override int Update(Application entity)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = this.connection;
                command.CommandText = @"UPDATE [application] SET 
                                               [display_name] = @displayName, 
                                               [secret] = @secret,
                                               [restricted_access_until] = @restricted_access_until
                                         WHERE [application_id] = @application_id ";

                command.Parameters.Add(CreateParameter(command, "application_id", entity.ApplicationID));
                command.Parameters.Add(CreateParameter(command, "displayName", entity.DisplayName));
                command.Parameters.Add(CreateParameter(command, "secret", entity.Secret));
                command.Parameters.Add(CreateParameter(command, "restricted_access_until", entity.RestrictedAccessUntil == DateTime.MinValue ? (object)DBNull.Value : entity.RestrictedAccessUntil));

                return command.ExecuteNonQuery();
            }
        }

        internal override int Delete(Application entity)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = this.connection;
                command.CommandText = @"DELETE [application]
                                         WHERE [application_id] = @application_id ";

                command.Parameters.Add(CreateParameter(command, "application_id", entity.ApplicationID));
                return command.ExecuteNonQuery();
            }
        }

        internal override IEnumerable<Application> Retrieve(Application entity)
        {
            List<Application> applicationList = new List<Application>();

            using (SqlCommand command = new SqlCommand())
            {
                //------------------------------------------------------------------------------------------------------
                // Command Build
                command.Connection = this.connection;
                command.CommandText = @"SELECT [application_id], [display_name], [secret], [restricted_access_until]
                                          FROM [application] ";

                if (entity != null && !string.IsNullOrEmpty(entity.ApplicationID))
                {
                    command.CommandText += (command.Parameters.Count == 0 ? "WHERE " : "AND ") + "[application_id] = @application_id ";
                    command.Parameters.Add(CreateParameter(command, "application_id", entity.ApplicationID));
                }

                if (entity != null && !string.IsNullOrEmpty(entity.DisplayName))
                {
                    command.CommandText += (command.Parameters.Count == 0 ? "WHERE " : "AND ") + "[display_name] = @display_name ";
                    command.Parameters.Add(CreateParameter(command, "display_name", entity.DisplayName));
                }

                if (entity != null && !string.IsNullOrEmpty(entity.Secret))
                {
                    command.CommandText += (command.Parameters.Count == 0 ? "WHERE " : "AND ") + "[secret] = @secret ";
                    command.Parameters.Add(CreateParameter(command, "secret", entity.Secret));
                }

                if (entity != null && entity.RestrictedAccessUntil != DateTime.MinValue)
                {
                    command.CommandText += (command.Parameters.Count == 0 ? "WHERE " : "AND ") + "[restricted_access_until] = @restricted_access_until ";
                    command.Parameters.Add(CreateParameter(command, "restricted_access_until", entity.RestrictedAccessUntil == DateTime.MinValue ? (object)DBNull.Value : entity.RestrictedAccessUntil));
                }

                //------------------------------------------------------------------------------------------------------
                // Reader
                SqlDataReader reader = command.ExecuteReader();

                bool read = reader.Read();

                while (read)
                {
                    applicationList.Add(new Application(
                        (string)reader["application_id"],
                        (string)reader["display_name"],
                        (string)reader["secret"],
                        reader["restricted_access_until"] is DBNull ? DateTime.MinValue : (DateTime)reader["restricted_access_until"]));

                    read = reader.Read();
                }

                reader.Close();
            }

            return applicationList;
        }
    }
}
