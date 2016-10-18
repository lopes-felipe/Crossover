using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageLogger.Entities;

namespace MessageLogger.Test.Mock
{
    public class ApplicationDataAccessMock
        : Data.DataAccess<Entities.Application>
    {
        public ApplicationDataAccessMock()
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            // Repository
            this.repository = new List<Application>();
            this.repository.Add(new Application("1", "1", "1"));
            this.repository.Add(new Application("2", "2", "2"));
            this.repository.Add(new Application("3", "3", "3"));
        }

        private List<Application> repository = null;

        internal override Application Create(Application entity)
        {
            this.repository.Add(entity);
            return entity;
        }

        internal override int Delete(Application entity)
        {
            this.repository.Remove(this.repository.First(item => item.ApplicationID == entity.ApplicationID));
            return 1;
        }
        
        internal override IEnumerable<Application> Retrieve(Application entity)
        {
            if (entity == null)
                return this.repository;

            //----------------------------------------------------------------------------------------------------------------------------------
            // Filters
            IEnumerable<Application> resultRepository = this.repository;

            if (!string.IsNullOrEmpty(entity.ApplicationID))
                resultRepository = resultRepository.Where(app => app.ApplicationID == entity.ApplicationID);

            if (!string.IsNullOrEmpty(entity.DisplayName))
                resultRepository = resultRepository.Where(app => app.DisplayName == entity.DisplayName);

            if (!string.IsNullOrEmpty(entity.Secret))
                resultRepository = resultRepository.Where(app => app.Secret == entity.Secret);

            return resultRepository;
        }

        internal override int Update(Application entity)
        {
            Application element = this.repository.First(item => item.ApplicationID == entity.ApplicationID);
            this.repository[this.repository.IndexOf(element)] = entity;

            return 1;
        }
    }
}
