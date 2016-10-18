using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageLogger.Entities;
using System.Data.SqlClient;

namespace MessageLogger.Data
{
    public class ApplicationCallDataAccess
        : DataAccess<ApplicationCall>
    {
        public ApplicationCallDataAccess(SqlConnection connection)
        {
            this.connection = connection;
        }

        private SqlConnection connection = null;

        internal override ApplicationCall Create(ApplicationCall entity)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = this.connection;
                command.CommandText = @"INSERT INTO [application_call]
                                                   ([application_id], [call_date])
                                            VALUES (@application_id, @call_date) 
                                            SELECT @@IDENTITY ";

                command.Parameters.Add(CreateParameter(command, "application_id", entity.ApplicationID));
                command.Parameters.Add(CreateParameter(command, "call_date", entity.CallDate));

                entity.CallID = command.ExecuteNonQuery();

                return entity;
            }
        }

        internal override int Delete(ApplicationCall entity)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = this.connection;
                command.CommandText = @"DELETE [application_call]
                                         WHERE [call_id] = @call_id ";

                command.Parameters.Add(CreateParameter(command, "call_id", entity.CallID));
                return command.ExecuteNonQuery();
            }
        }

        internal override int Update(ApplicationCall entity)
        {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = this.connection;
                command.CommandText = @"UPDATE [application_call] SET 
                                               [application_id] = @application_id,
                                               [call_date] = @call_date
                                         WHERE [call_id] = @call_id ";

                command.Parameters.Add(CreateParameter(command, "application_id", entity.ApplicationID));
                command.Parameters.Add(CreateParameter(command, "call_date", entity.CallDate));
                command.Parameters.Add(CreateParameter(command, "call_id", entity.CallID));

                return command.ExecuteNonQuery();
            }
        }

        internal override IEnumerable<ApplicationCall>Retrieve(ApplicationCall entity)
        {
            using (SqlCommand command = BuildCommand(entity))
                return ExecuteReader(command);
        }

        internal IEnumerable<ApplicationCall> Retrieve(ApplicationCall entity, DateTime callDateFrom, DateTime callDateUntil)
        {
            using (SqlCommand command = BuildCommand(entity))
            {
                if (callDateFrom != DateTime.MinValue)
                {
                    command.CommandText += (command.Parameters.Count == 0 ? "WHERE " : "AND ") + "@call_date_from <= [call_date] ";
                    command.Parameters.Add(CreateParameter(command, "call_date_from", callDateFrom));
                }

                if (callDateUntil != DateTime.MinValue)
                {
                    command.CommandText += (command.Parameters.Count == 0 ? "WHERE " : "AND ") + "[call_date] <= @call_date_until ";
                    command.Parameters.Add(CreateParameter(command, "call_date_until", callDateUntil));
                }

                return ExecuteReader(command);
            }
        }

        private SqlCommand BuildCommand(ApplicationCall entity)
        {
            //------------------------------------------------------------------------------------------------------
            // Command Build
            SqlCommand command = new SqlCommand();

            command.Connection = this.connection;
            command.CommandText = @"SELECT [call_id], [application_id], [call_date]
                                          FROM [application_call] ";
            
            if (entity != null && entity.CallID > 0)
            {
                command.CommandText += (command.Parameters.Count == 0 ? "WHERE " : "AND ") + "[call_id] = @call_id ";
                command.Parameters.Add(CreateParameter(command, "call_id", entity.CallID));
            }

            if (entity != null && !string.IsNullOrEmpty(entity.ApplicationID))
            {
                command.CommandText += (command.Parameters.Count == 0 ? "WHERE " : "AND ") + "[application_id] = @application_id ";
                command.Parameters.Add(CreateParameter(command, "application_id", entity.ApplicationID));
            }

            if (entity != null && entity.CallDate != DateTime.MinValue)
            {
                command.CommandText += (command.Parameters.Count == 0 ? "WHERE " : "AND ") + "[call_date] = @call_date ";
                command.Parameters.Add(CreateParameter(command, "call_date", entity.CallDate));
            }

            return command;
        }

        private List<ApplicationCall> ExecuteReader(SqlCommand command)
        {
            List<ApplicationCall> applicationCallList = new List<ApplicationCall>();

            //------------------------------------------------------------------------------------------------------
            // Reader
            SqlDataReader reader = command.ExecuteReader();

            bool read = reader.Read();

            while (read)
            {
                applicationCallList.Add(new ApplicationCall(
                    (long)reader["call_id"],
                    (string)reader["application_id"],
                    (DateTime)reader["call_date"]));

                read = reader.Read();
            }

            reader.Close();

            return applicationCallList;
        }
    }
}
