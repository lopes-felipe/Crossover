using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLogger.Data
{
    public abstract class DataAccess<TEntity>
    {
        internal abstract TEntity Create(TEntity entity);

        internal abstract int Update(TEntity entity);

        internal abstract int Delete(TEntity entity);

        internal IEnumerable<TEntity> Retrieve()
        {
            return this.Retrieve(default(TEntity));
        }

        internal abstract IEnumerable<TEntity> Retrieve(TEntity entity);

        protected DbParameter CreateParameter(DbCommand command, string parameterName, object value)
        {
            DbParameter parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value;

            return parameter;
        }
    }
}
