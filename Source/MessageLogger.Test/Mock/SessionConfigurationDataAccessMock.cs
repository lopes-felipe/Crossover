using MessageLogger.Data;
using MessageLogger.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLogger.Test.Mock
{
    public class SessionConfigurationDataAccessMock
        : DataAccess<SessionConfiguration>
    {
        public SessionConfigurationDataAccessMock()
        {
            //----------------------------------------------------------------------------------------------------------------------------------------------------------------
            // Repository
            this.repository = new List<SessionConfiguration>();
            this.repository.Add(new SessionConfiguration(1, 60));
        }

        private List<SessionConfiguration> repository = null;

        internal override SessionConfiguration Create(SessionConfiguration entity)
        {
            this.repository.Add(entity);
            return entity;
        }

        internal override int Delete(SessionConfiguration entity)
        {
            this.repository.Remove(this.repository.First(item => item.ConfigurationID == entity.ConfigurationID));
            return 1;
        }

        internal override IEnumerable<SessionConfiguration> Retrieve(SessionConfiguration entity)
        {
            if (entity == null)
                return this.repository;

            //----------------------------------------------------------------------------------------------------------------------------------------------------------------
            // Filters
            IEnumerable<SessionConfiguration> resultRepository = this.repository;

            if (entity.ConfigurationID > 0)
                resultRepository = resultRepository.Where(app => app.ConfigurationID == entity.ConfigurationID);

            if (entity.SessionLifetimeMinutes > 0)
                resultRepository = resultRepository.Where(app => app.SessionLifetimeMinutes == entity.SessionLifetimeMinutes);

            return resultRepository;
        }

        internal override int Update(SessionConfiguration entity)
        {
            SessionConfiguration element = this.repository.First(item => item.ConfigurationID == entity.ConfigurationID);
            this.repository[this.repository.IndexOf(element)] = entity;

            return 1;
        }
    }
}
