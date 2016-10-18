using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageLogger.Entities;

namespace MessageLogger.Data
{
    public class LogDataAccess
        : DataAccess<Log>
    {
        public LogDataAccess(SqlConnection connection)
        {
            this.connection = connection;
        }

        private SqlConnection connection = null;

        internal override Log Create(Log entity)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = this.connection;
                command.CommandText = @"INSERT INTO [log]
                                                   ([logger], [level], [message], [application_id])
                                        VALUES     (@logger, @level, @message, @application_id) 
                                        SELECT @@IDENTITY ";

                command.Parameters.Add(CreateParameter(command, "logger", entity.Logger));
                command.Parameters.Add(CreateParameter(command, "level", entity.Level));
                command.Parameters.Add(CreateParameter(command, "message", entity.Message));
                command.Parameters.Add(CreateParameter(command, "application_id", string.IsNullOrEmpty(entity.ApplicationID) ? (object)DBNull.Value : entity.ApplicationID));

                entity.LogID = command.ExecuteNonQuery();

                return entity;
            }
        }

        internal override int Update(Log entity)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = this.connection;
                command.CommandText = @"UPDATE [log] SET 
                                               [logger] = @logger, 
                                               [level] = @level,
                                               [message] = @message,
                                               [application_id] = @application_id
                                         WHERE [log_id] = @log_id ";
                
                command.Parameters.Add(CreateParameter(command, "logger", entity.Logger));
                command.Parameters.Add(CreateParameter(command, "level", entity.Level));
                command.Parameters.Add(CreateParameter(command, "message", entity.Message));
                command.Parameters.Add(CreateParameter(command, "application_id", string.IsNullOrEmpty(entity.ApplicationID) ? (object)DBNull.Value : entity.ApplicationID));
                command.Parameters.Add(CreateParameter(command, "log_id", entity.LogID));

                return command.ExecuteNonQuery();
            }
        }

        internal override int Delete(Log entity)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = this.connection;
                command.CommandText = @"DELETE [log]
                                         WHERE [log_id] = @log_id ";

                command.Parameters.Add(CreateParameter(command, "log_id", entity.LogID));
                return command.ExecuteNonQuery();
            }
        }

        internal override IEnumerable<Log> Retrieve(Log entity)
        {
            List<Log> logList = new List<Log>();

            using (SqlCommand command = new SqlCommand())
            {
                //------------------------------------------------------------------------------------------------------
                // Command Build
                command.Connection = this.connection;
                command.CommandText = @"SELECT [log_id], [logger], [level], [message], [application_id] 
                                          FROM [log] ";

                if (entity != null && entity.LogID != 0)
                {
                    command.CommandText += (command.Parameters.Count == 0 ? "WHERE " : "AND ") + "[log_id] = @log_id ";
                    command.Parameters.Add(CreateParameter(command, "log_id", entity.LogID));
                }

                if (entity != null && !string.IsNullOrEmpty(entity.ApplicationID))
                {
                    command.CommandText += (command.Parameters.Count == 0 ? "WHERE " : "AND ") + "[application_id] = @application_id ";
                    command.Parameters.Add(CreateParameter(command, "application_id", entity.ApplicationID));
                }

                if (entity != null && !string.IsNullOrEmpty(entity.Message))
                {
                    command.CommandText += (command.Parameters.Count == 0 ? "WHERE " : "AND ") + "[message] = @message ";
                    command.Parameters.Add(CreateParameter(command, "message", entity.Message));
                }

                if (entity != null && !string.IsNullOrEmpty(entity.Level))
                {
                    command.CommandText += (command.Parameters.Count == 0 ? "WHERE " : "AND ") + "[level] = @level ";
                    command.Parameters.Add(CreateParameter(command, "level", entity.Level));
                }

                if (entity != null && !string.IsNullOrEmpty(entity.Logger))
                {
                    command.CommandText += (command.Parameters.Count == 0 ? "WHERE " : "AND ") + "[logger] = @logger ";
                    command.Parameters.Add(CreateParameter(command, "logger", entity.Logger));
                }

                //------------------------------------------------------------------------------------------------------
                // Reader
                SqlDataReader reader = command.ExecuteReader();

                bool read = reader.Read();

                while (read)
                {
                    logList.Add(new Log(
                        (int)reader["log_id"],
                        (string)reader["logger"],
                        (string)reader["level"],
                        (string)reader["message"],
                        reader["application_id"] is DBNull ? string.Empty : (string)reader["application_id"]));

                    read = reader.Read();
                }

                reader.Close();
            }

            return logList;
        }
    }
}
