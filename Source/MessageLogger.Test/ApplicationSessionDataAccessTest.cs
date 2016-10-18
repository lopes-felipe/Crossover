using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageLogger.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace MessageLogger.Test
{
    [TestClass]
    public class ApplicationSessionDataAccessTest
    {
        [TestInitialize]
        public void TestIntialize()
        {
            this.connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Crossover"].ConnectionString);
            this.connection.Open();
        }

        private SqlConnection connection;

        [TestCleanup]
        public void TestCleanup()
        {
            this.connection.Dispose();
            this.connection = null;
        }

        [TestMethod]
        public void CreateTest()
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            // Setup
            Entities.ApplicationSession applicationSession = new Entities.ApplicationSession(0, "1", Guid.Parse("5D12D441-3EAB-49B9-95F4-605A4A0E21CA"), true, DateTime.Now, DateTime.Now);
            Entities.ApplicationSession secondApplicationSession = new Entities.ApplicationSession(0, "1", Guid.Parse("5D894E6E-9982-41DC-BF25-35F5288956ED"), false, DateTime.Now, DateTime.Now);

            ApplicationSessionDataAccess applicationSessionDataAccess = new ApplicationSessionDataAccess(this.connection);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            applicationSession = applicationSessionDataAccess.Create(applicationSession);
            secondApplicationSession = applicationSessionDataAccess.Create(secondApplicationSession);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Assert
            Assert.IsTrue(applicationSession.SessionID > 0);
            Assert.IsTrue(secondApplicationSession.SessionID > 0);
        }

        [TestMethod]
        public void UpdateTest()
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            // Setup
            Entities.ApplicationSession applicationSession = new Entities.ApplicationSession(1, "1", Guid.Parse("5D12D441-3EAB-49B9-95F4-605A4A0E21CA"), true, DateTime.Now, DateTime.Now);
            ApplicationSessionDataAccess applicationSessionDataAccess = new ApplicationSessionDataAccess(this.connection);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            int affectedRows = applicationSessionDataAccess.Update(applicationSession);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Assert
            Assert.IsTrue(affectedRows == 1);
        }


        [TestMethod]
        public void DeleteTest()
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            // Setup
            Entities.ApplicationSession applicationSession = new Entities.ApplicationSession(1);
            Entities.ApplicationSession secondApplicationSession = new Entities.ApplicationSession(2);

            ApplicationSessionDataAccess applicationSessionDataAccess = new ApplicationSessionDataAccess(this.connection);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            int affectedRows = applicationSessionDataAccess.Delete(applicationSession);
            int secondAffectedRows = applicationSessionDataAccess.Delete(secondApplicationSession);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Assert
            Assert.IsTrue(affectedRows == 1);
            Assert.IsTrue(secondAffectedRows == 1);
        }
        
        [TestMethod]
        public void RetrieveAllTest()
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            // Setup
            ApplicationSessionDataAccess applicationSessionDataAccess = new ApplicationSessionDataAccess(this.connection);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            IEnumerable<Entities.ApplicationSession> applicationSessions = applicationSessionDataAccess.Retrieve();

            //----------------------------------------------------------------------------------------------------------------------------------
            // Assert
            Assert.IsTrue(applicationSessions.Count() == 2);
        }

        [TestMethod]
        public void RetrieveByID()
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            // Setup
            Entities.ApplicationSession applicationSession = new Entities.ApplicationSession(1);
            ApplicationSessionDataAccess applicationSessionDataAccess = new ApplicationSessionDataAccess(this.connection);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            IEnumerable<Entities.ApplicationSession> applicationSessions = applicationSessionDataAccess.Retrieve(applicationSession);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Assert
            Assert.IsTrue(applicationSessions.Count() == 1);
            Assert.IsTrue(applicationSessions.First().SessionID == 1);
        }
    }
}
