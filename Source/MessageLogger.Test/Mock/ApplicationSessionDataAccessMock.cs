using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageLogger.Entities;

namespace MessageLogger.Test.Mock
{
    public class ApplicationSessionDataAccessMock
        : Data.DataAccess<Entities.ApplicationSession>
    {
        public ApplicationSessionDataAccessMock()
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            // Repository
            this.repository = new List<ApplicationSession>();
            this.repository.Add(new ApplicationSession(1, "1", Guid.Parse("207ECD15-4D10-4177-A818-066E71EB9208"), false, DateTime.Now, DateTime.Now.AddDays(-1)));
            this.repository.Add(new ApplicationSession(2, "1", Guid.Parse("426D6C0E-6140-43F5-8995-DAFF90617A8B"), true, DateTime.Now, DateTime.Now));

            this.repository.Add(new ApplicationSession(3, "2", Guid.Parse("5650EE3C-4F92-40C4-B8D6-B3D4609CF966"), false, DateTime.Now, DateTime.Now));
            this.repository.Add(new ApplicationSession(4, "2", Guid.Parse("79B9D3FE-57A9-4736-810C-22AF672198CB"), true, DateTime.Now, DateTime.Now.AddDays(1)));
        }

        private List<ApplicationSession> repository = null;

        internal override ApplicationSession Create(ApplicationSession entity)
        {
            this.repository.Add(entity);
            return entity;
        }

        internal override int Delete(ApplicationSession entity)
        {
            this.repository.Remove(this.repository.First(item => item.SessionID == entity.SessionID));
            return 1;
        }

        internal override IEnumerable<ApplicationSession> Retrieve(ApplicationSession entity)
        {
            if (entity == null)
                return this.repository;

            //----------------------------------------------------------------------------------------------------------------------------------
            // Filter
            IEnumerable<ApplicationSession> resultRepository = this.repository;

            if (entity.SessionID != 0)
                resultRepository = resultRepository.Where(app => app.SessionID == entity.SessionID);

            if (!string.IsNullOrEmpty(entity.ApplicationID))
                resultRepository = resultRepository.Where(app => app.ApplicationID == entity.ApplicationID);
            
            if (entity.AccessToken != Guid.Empty)
                resultRepository = resultRepository.Where(app => app.AccessToken == entity.AccessToken);

            if (entity.Active != null)
                resultRepository = resultRepository.Where(app => app.Active == entity.Active);

            if (entity.CreatedDate != DateTime.MinValue)
                resultRepository = resultRepository.Where(app => app.CreatedDate == entity.CreatedDate);

            if (entity.ValidUntil != DateTime.MinValue)
                resultRepository = resultRepository.Where(app => app.ValidUntil == entity.ValidUntil);

            return resultRepository;
        }

        internal override int Update(ApplicationSession entity)
        {
            ApplicationSession element = this.repository.First(item => item.SessionID == entity.SessionID);
            this.repository[this.repository.IndexOf(element)] = entity;

            return 1;
        }
    }
}
