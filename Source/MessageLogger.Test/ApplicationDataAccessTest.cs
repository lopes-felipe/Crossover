using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MessageLogger.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MessageLogger.Test
{
    [TestClass]
    public class ApplicationDataAccessTest
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
            Entities.Application application = new Entities.Application("1", "Test", "test", DateTime.MinValue);
            Entities.Application secondApplication = new Entities.Application("2", "Test2", "test2", DateTime.MinValue);

            ApplicationDataAccess applicationDataAccess = new ApplicationDataAccess(this.connection);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            application = applicationDataAccess.Create(application);
            secondApplication = applicationDataAccess.Create(secondApplication);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Assert
            Assert.IsNotNull(application);
            Assert.IsNotNull(secondApplication);
        }

        [TestMethod]
        public void UpdateTest()
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            // Setup
            Entities.Application application = new Entities.Application("1", "Test2", "test2", DateTime.Now);
            ApplicationDataAccess applicationDataAccess = new ApplicationDataAccess(this.connection);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            int affectedRows = applicationDataAccess.Update(application);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Assert
            Assert.IsTrue(affectedRows == 1);
        }


        [TestMethod]
        public void DeleteTest()
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            // Setup
            Entities.Application application = new Entities.Application("1");
            Entities.Application secondApplication = new Entities.Application("2");

            ApplicationDataAccess applicationDataAccess = new ApplicationDataAccess(this.connection);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            int affectedRows = applicationDataAccess.Delete(application);
            int secondAffectedRows = applicationDataAccess.Delete(secondApplication);

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
            ApplicationDataAccess applicationDataAccess = new ApplicationDataAccess(this.connection);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            IEnumerable<Entities.Application> applications = applicationDataAccess.Retrieve();

            //----------------------------------------------------------------------------------------------------------------------------------
            // Assert
            Assert.IsTrue(applications.Count() == 2);
        }

        [TestMethod]
        public void RetrieveByID()
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            // Setup
            Entities.Application application = new Entities.Application("1");
            ApplicationDataAccess applicationDataAccess = new ApplicationDataAccess(this.connection);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            IEnumerable<Entities.Application> applications = applicationDataAccess.Retrieve(application);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Assert
            Assert.IsTrue(applications.Count() == 1);
            Assert.IsTrue(applications.First().ApplicationID == "1");
        }
    }
}
