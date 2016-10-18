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
    public class SessionConfigurationDataAccessTest
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
            Entities.SessionConfiguration sessionConfiguration = new Entities.SessionConfiguration(2, 90);
            Entities.SessionConfiguration secondApplicationSession = new Entities.SessionConfiguration(3, 120);

            SessionConfigurationDataAccess sessionConfigurationDataAccess = new SessionConfigurationDataAccess(this.connection);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            sessionConfiguration = sessionConfigurationDataAccess.Create(sessionConfiguration);
            secondApplicationSession = sessionConfigurationDataAccess.Create(secondApplicationSession);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Assert
            Assert.IsNotNull(sessionConfiguration);
            Assert.IsNotNull(secondApplicationSession);
        }

        [TestMethod]
        public void UpdateTest()
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            // Setup
            Entities.SessionConfiguration sessionConfiguration = new Entities.SessionConfiguration(2, 91);
            SessionConfigurationDataAccess sessionConfigurationDataAccess = new SessionConfigurationDataAccess(this.connection);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            int affectedRows = sessionConfigurationDataAccess.Update(sessionConfiguration);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Assert
            Assert.IsTrue(affectedRows == 1);
        }


        [TestMethod]
        public void DeleteTest()
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            // Setup
            Entities.SessionConfiguration sessionConfiguration = new Entities.SessionConfiguration(2);
            Entities.SessionConfiguration secondApplicationSession = new Entities.SessionConfiguration(3);

            SessionConfigurationDataAccess sessionConfigurationDataAccess = new SessionConfigurationDataAccess(this.connection);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            int affectedRows = sessionConfigurationDataAccess.Delete(sessionConfiguration);
            int secondAffectedRows = sessionConfigurationDataAccess.Delete(secondApplicationSession);

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
            SessionConfigurationDataAccess sessionConfigurationDataAccess = new SessionConfigurationDataAccess(this.connection);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            IEnumerable<Entities.SessionConfiguration> sessionConfigurations = sessionConfigurationDataAccess.Retrieve();

            //----------------------------------------------------------------------------------------------------------------------------------
            // Assert
            Assert.IsTrue(sessionConfigurations.Count() == 3);
        }

        [TestMethod]
        public void RetrieveByID()
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            // Setup
            Entities.SessionConfiguration sessionConfiguration = new Entities.SessionConfiguration(1);
            SessionConfigurationDataAccess sessionConfigurationDataAccess = new SessionConfigurationDataAccess(this.connection);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            IEnumerable<Entities.SessionConfiguration> sessionConfigurations = sessionConfigurationDataAccess.Retrieve(sessionConfiguration);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Assert
            Assert.IsTrue(sessionConfigurations.Count() == 1);
            Assert.IsTrue(sessionConfigurations.First().ConfigurationID == 1);
        }
    }
}
