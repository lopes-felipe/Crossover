using MessageLogger.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLogger.Test
{
    [TestClass]
    public class ApplicationCallDataAccessTest
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
            Entities.ApplicationCall applicationCall = new Entities.ApplicationCall(0, "1", DateTime.Now);
            Entities.ApplicationCall secondApplicationCall = new Entities.ApplicationCall(0, "1", DateTime.Now);

            ApplicationCallDataAccess applicationCallDataAccess = new ApplicationCallDataAccess(this.connection);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            applicationCall = applicationCallDataAccess.Create(applicationCall);
            secondApplicationCall = applicationCallDataAccess.Create(secondApplicationCall);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Assert
            Assert.IsTrue(applicationCall.CallID > 0);
            Assert.IsTrue(secondApplicationCall.CallID > 0);
        }

        [TestMethod]
        public void UpdateTest()
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            // Setup
            Entities.ApplicationCall applicationCall = new Entities.ApplicationCall(2, "1", DateTime.Now);
            ApplicationCallDataAccess applicationCallDataAccess = new ApplicationCallDataAccess(this.connection);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            int affectedRows = applicationCallDataAccess.Update(applicationCall);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Assert
            Assert.IsTrue(affectedRows == 1);
        }


        [TestMethod]
        public void DeleteTest()
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            // Setup
            Entities.ApplicationCall applicationCall = new Entities.ApplicationCall(1);
            Entities.ApplicationCall secondApplicationCall = new Entities.ApplicationCall(2);

            ApplicationCallDataAccess applicationCallDataAccess = new ApplicationCallDataAccess(this.connection);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            int affectedRows = applicationCallDataAccess.Delete(applicationCall);
            int secondAffectedRows = applicationCallDataAccess.Delete(secondApplicationCall);

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
            ApplicationCallDataAccess applicationCallDataAccess = new ApplicationCallDataAccess(this.connection);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            IEnumerable<Entities.ApplicationCall> applicationCalls = applicationCallDataAccess.Retrieve();

            //----------------------------------------------------------------------------------------------------------------------------------
            // Assert
            Assert.IsTrue(applicationCalls.Count() == 2);
        }

        [TestMethod]
        public void RetrieveByID()
        {
            //----------------------------------------------------------------------------------------------------------------------------------
            // Setup
            Entities.ApplicationCall applicationCall = new Entities.ApplicationCall(2);
            ApplicationCallDataAccess applicationCallDataAccess = new ApplicationCallDataAccess(this.connection);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Execute
            IEnumerable<Entities.ApplicationCall> applicationCalls = applicationCallDataAccess.Retrieve(applicationCall);

            //----------------------------------------------------------------------------------------------------------------------------------
            // Assert
            Assert.IsTrue(applicationCalls.Count() == 1);
            Assert.IsTrue(applicationCalls.First().CallID == 2);
        }
    }
}
